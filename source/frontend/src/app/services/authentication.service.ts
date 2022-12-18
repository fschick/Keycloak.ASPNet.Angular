import {Injectable} from '@angular/core';
import {from, Observable, of, switchMap, tap} from 'rxjs';
import {environment} from '../../environments/environment';
import {AuthConfig, OAuthService} from "angular-oauth2-oidc";

class UserInfo {
  preferred_username?: string
}

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private userInfo?: UserInfo;

  get isAuthenticated(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

  get username(): string | undefined {
    return this.userInfo?.preferred_username;
  }

  constructor(
    private oauthService: OAuthService,
  ) {
  }

  public init(): Observable<any> {
    const authConfig: AuthConfig = {
      scope: 'openid profile email offline_access',
      responseType: 'code',
      oidc: true,
      clientId: environment.clientId,
      issuer: environment.authority,
      redirectUri: location.origin,
      postLogoutRedirectUri: location.origin,
      requireHttps: false,
    };

    return this.initAuthService(authConfig)
      .pipe(
        switchMap(() => this.loadUserProfile(this.isAuthenticated))
      );
  }

  public getAccessToken(): Observable<string> {
    if (!this.oauthService.clientId)
      throw Error('Authentication service not initialized');

    return of(this.oauthService.getAccessToken());
  }

  public login(): void {
    this.oauthService.initCodeFlow();
  }

  public logout(): void {
    this.oauthService.logOut();
  }

  private initAuthService(authConfig: AuthConfig): Observable<any> {
    this.oauthService.configure(authConfig);
    this.oauthService.setupAutomaticSilentRefresh();
    this.oauthService.strictDiscoveryDocumentValidation = false;
    return from(this.oauthService.loadDiscoveryDocumentAndTryLogin());
  }

  private loadUserProfile(isAuthenticated: boolean): Observable<any> {
    return isAuthenticated
      ? from(this.oauthService.loadUserProfile()).pipe(tap((userInfo: any) => this.userInfo = userInfo.info))
      : of(undefined);
  }
}
