```typescript
import KeycloakAuthorization from 'keycloak-js/dist/keycloak-authz';

let keycloak: Keycloak;
let userIsAuthenticated = false;
let authorization: KeycloakAuthorization;

function initAuthorization(): Observable<void> {
  authorization = new KeycloakAuthorization(keycloak);
  authorization.init();
  return of(void 0);
}

export function keycloakLoaderFactory(): () => Promise<any> | Observable<any> {
  return () => {
    const openIdConnectOptions = {
      url: `http://${location.hostname}:8080`,
      realm: 'asp-net-keycloak',
      clientId: 'frontend',
    };

    keycloak = new Keycloak(openIdConnectOptions);
    
    const initOptions: KeycloakInitOptions = {
      onLoad: 'check-sso',
      silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html'
    };

    return from(keycloak.init(initOptions))
      .pipe(
        switchMap(isAuthenticated => {
          userIsAuthenticated = isAuthenticated;
          return userIsAuthenticated
            ? forkJoin([initAuthorization(), from(keycloak!.loadUserProfile())])
            : of(undefined);
        })
      );
  };
}
```