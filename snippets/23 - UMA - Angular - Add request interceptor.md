```typescript
@NgModule({
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthenticationInterceptor,
    multi: true
  }]
})
export class AppModule { }

let keycloak: Keycloak;
let userIsAuthenticated = false;
let authorization: KeycloakAuthorization;

class AuthenticationInterceptor implements HttpInterceptor {
  public intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (!userIsAuthenticated) {
      keycloak.login();
      return next.handle(request);
    }

    return this.getAccessToken()
      .pipe(mergeMap(token => {
        const authenticatedRequest = this.authenticate(request, token);
        return next.handle(authenticatedRequest);
      }))
  }

  private getAccessToken() {
    return from(keycloak.updateToken(5))
      .pipe(mergeMap(() => this.getRptToken()));
  }

  private getRptToken(): Observable<string> {
    return new Observable((observer: Observer<string>) => {
      authorization
        .entitlement('api')
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

  private authenticate<T>(request: HttpRequest<T>, token: string): HttpRequest<T> {
    return request.clone({ headers: request.headers.set('Authorization', `Bearer ${token}`) });
  }
}

```