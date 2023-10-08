```typescript
import {ApplicationConfig, provideZoneChangeDetection} from '@angular/core';
import {provideAppInitializer} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {from, Observable, of, switchMap} from "rxjs";
import Keycloak, {KeycloakInitOptions} from "keycloak-js";

let keycloak: Keycloak;

export const appConfig: ApplicationConfig = {
    providers: [
        // ...
        provideAppInitializer(() => keycloakLoaderFactory())
    ]
};

export function keycloakLoaderFactory(): Observable<unknown> {
    const openIdConnectOptions = {
        url: `http://${location.hostname}:8080`,
        realm: 'asp-net-keycloak',
        clientId: 'api',
    };

    keycloak = new Keycloak(openIdConnectOptions);

    const initOptions: KeycloakInitOptions = {
        onLoad: 'check-sso',
        silentCheckSsoRedirectUri: window.location.origin + '/public/silent-check-sso.html'
    };

    return from(keycloak.init(initOptions))
        .pipe(
            switchMap(isAuthenticated => isAuthenticated ? from(keycloak!.loadUserProfile()) : of(undefined)
            )
        );
}
```