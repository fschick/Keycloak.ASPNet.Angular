import {Injectable} from '@angular/core';
import {HttpRequest, HttpHandler, HttpEvent, HttpInterceptor} from '@angular/common/http';
import {mergeMap, Observable} from 'rxjs';
import {AuthenticationService} from './authentication.service';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

  constructor(
    private authenticationService: AuthenticationService,
  ) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (!this.authenticationService.isAuthenticated)
      return next.handle(request);

    return this.authenticationService
      .getAccessToken()
      .pipe(mergeMap(token => {
        const authenticatedRequest = this.authenticate(request, token);
        return next.handle(authenticatedRequest);
      }))
  }

  private authenticate<T>(request: HttpRequest<T>, token: string): HttpRequest<T> {
    return request.clone({headers: request.headers.set('Authorization', `Bearer ${token}`)});
  }
}
