```typescript
import Keycloak, { KeycloakInitOptions } from 'keycloak-js';

@NgModule({
  providers: [{
    provide: APP_INITIALIZER,
    useFactory: keycloakLoaderFactory,
    multi: true
  }]
})
export class AppModule { }

let keycloak: Keycloak;
let userIsAuthenticated = false;

export function keycloakLoaderFactory(): () => Promise<any> | Observable<any> {
  return () => {
    const openIdConnectOptions = {
      url: `http://${location.hostname}:8080`,
      realm: 'asp-net-keycloak',
      clientId: 'api',
    };

    const keycloak = new Keycloak(openIdConnectOptions);

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