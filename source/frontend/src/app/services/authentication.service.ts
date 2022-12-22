import {Injectable} from '@angular/core';
import Keycloak, {KeycloakInitOptions} from 'keycloak-js';
import {forkJoin, from, mergeMap, Observable, Observer, of} from 'rxjs';
import {map, switchMap} from 'rxjs/operators';
import {environment} from '../../environments/environment';
import KeycloakAuthorization from "keycloak-js/authz";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private keycloak?: Keycloak;
  private authorization?: KeycloakAuthorization;

  get isAuthenticated(): boolean {
    return this.keycloak?.authenticated ?? false;
  }

  get username(): string | undefined {
    return this.keycloak?.profile?.username;
  }

  public init(): Observable<any> {
    const openIdConnectOptions = {
      url: environment.authServerUrl,
      realm: environment.realm,
      clientId: environment.clientId,
    };

    this.keycloak = new Keycloak(openIdConnectOptions);
    const initKeycloak = this.initKeycloak(this.keycloak)
      .pipe(
        switchMap(isAuthenticated =>
          forkJoin([this.initAuthorization(), this.loadUserProfile(isAuthenticated)])
        )
      );

    return initKeycloak;
  }

  public getAccessToken(): Observable<string> {
    if (!this.keycloak)
      throw Error('Keycloak authentication service not initialized');

    const accessToken = from(this.keycloak.updateToken(5))
      .pipe(mergeMap(() => this.getRptToken()));

    return accessToken;
  }

  public login(): void {
    if (!this.keycloak)
      throw Error('Keycloak authentication service not initialized');
    this.keycloak.login();
  }

  public logout(): void {
    if (!this.keycloak)
      throw Error('Keycloak authentication service not initialized');
    this.keycloak.logout();
  }

  private initKeycloak(keycloak: Keycloak): Observable<any> {
    const initOptions: KeycloakInitOptions = {onLoad: 'check-sso', silentCheckSsoRedirectUri: window.location.origin + '/silent-check-sso.html'};
    return from(keycloak.init(initOptions));
  }

  private initAuthorization(): Observable<void> {
    if (!this.keycloak)
      throw Error('Keycloak authentication service not initialized');

    this.authorization = new KeycloakAuthorization(this.keycloak);
    this.authorization.init();
    return of(void 0);
  }

  private loadUserProfile(isAuthenticated: boolean): Observable<any> {
    return isAuthenticated ? from(this.keycloak!.loadUserProfile()) : of(undefined);
  }

  private getRptToken(): Observable<string> {
    return new Observable((observer: Observer<string>) => {
      if (!this.authorization)
        throw Error('Keycloak authorization service not initialized');

      this.authorization
        .entitlement(environment.resourceServerId)
        .then(
          rptToken => {
            observer.next(rptToken);
            observer.complete();
          },
          () => observer.error('Authorization request was denied by the server.'),
          () => observer.error('Could not obtain authorization data from server.')
        );
    });
  }
}
