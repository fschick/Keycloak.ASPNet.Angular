```typescript
import {APP_INITIALIZER, ApplicationConfig} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import Keycloak, {KeycloakInitOptions} from "keycloak-js";
import {from, Observable, of, switchMap} from "rxjs";

let keycloak: Keycloak;

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    {
      provide: APP_INITIALIZER,
      useFactory: keycloakLoaderFactory,
      multi: true,
      deps: [],
    }
  ]
};

export function keycloakLoaderFactory(): () => Promise<any> | Observable<any> {
  return () => {
    const openIdConnectOptions = {
      url: `http://${location.hostname}:8080`,
      realm: 'asp-net-keycloak',
      clientId: 'api',
    };

    keycloak = new Keycloak(openIdConnectOptions);

    const initOptions: KeycloakInitOptions = {
      onLoad: 'check-sso',
      silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html'
    };

    return from(keycloak.init(initOptions))
      .pipe(
        switchMap(isAuthenticated => isAuthenticated ? from(keycloak!.loadUserProfile()) : of(undefined)
        )
      );
  };
}
```