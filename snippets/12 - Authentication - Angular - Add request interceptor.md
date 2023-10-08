```typescript
import {APP_INITIALIZER, ApplicationConfig, Injectable} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import Keycloak, {KeycloakInitOptions} from "keycloak-js";
import {from, map, mergeMap, Observable, of, switchMap} from "rxjs";
import {HttpEvent, HttpHandlerFn, HttpRequest, provideHttpClient, withInterceptors} from "@angular/common/http";

let keycloak: Keycloak;

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([authenticationInterceptor])),
    ...
  ]
};

export function keycloakLoaderFactory(): () => Promise<any> | Observable<any> {
  // ...
}

export function authenticationInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  function getAccessToken() {
    return from(keycloak.updateToken(5))
      .pipe(map(() => {
        return keycloak!.token ?? '';
      }));
  }

  function authenticate<T>(request: HttpRequest<T>, token: string): HttpRequest<T> {
    return request.clone({headers: request.headers.set('Authorization', `Bearer ${token}`)});
  }

  if (!keycloak.authenticated) {
    return next(req);
  }

  return getAccessToken()
    .pipe(mergeMap(token => {
      const authenticatedRequest = authenticate(req, token);
      return next(authenticatedRequest);
    }))
}

@Injectable({providedIn: 'root'})
export class AuthService {
  public login() {
    keycloak.login();
  }

  public logout() {
    keycloak.logout();
  }
}
```