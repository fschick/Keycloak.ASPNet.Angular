import {Injectable} from '@angular/core';
import Keycloak, {KeycloakInitOptions} from 'keycloak-js';
import {from, Observable, of} from 'rxjs';
import {map, switchMap} from 'rxjs/operators';
import {environment} from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private keycloak?: Keycloak;

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
    return this.initKeycloak(this.keycloak)
      .pipe(
        switchMap(isAuthenticated => this.loadUserProfile(isAuthenticated)
        )
      );
  }

  public getAccessToken(): Observable<string> {
    if (!this.keycloak)
      throw Error('Keycloak authentication service not initialized');

    return from(this.keycloak.updateToken(5))
      .pipe(map(() => this.keycloak!.token ?? ''));
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
    const initOptions: KeycloakInitOptions = {onLoad: 'check-sso', silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html'};
    return from(keycloak.init(initOptions));
  }

  private loadUserProfile(isAuthenticated: boolean): Observable<any> {
    return isAuthenticated ? from(this.keycloak!.loadUserProfile()) : of(undefined);
  }
}
