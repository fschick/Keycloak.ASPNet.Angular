import {HttpInterceptorFn, HttpRequest} from '@angular/common/http';
import {AuthenticationService} from "./authentication.service";
import {inject} from "@angular/core";
import {mergeMap} from "rxjs";

export const authenticationInterceptor: HttpInterceptorFn = (req, next) => {
  const authenticationService = inject(AuthenticationService);
  if (!authenticationService.isAuthenticated)
    return next(req);

  return authenticationService
      .getAccessToken()
      .pipe(mergeMap(token => {
      const authenticatedRequest = authenticate(req, token);
      return next(authenticatedRequest);
      }))

  return next(req);
};

function authenticate<T>(request: HttpRequest<T>, token: string): HttpRequest<T> {
    return request.clone({headers: request.headers.set('Authorization', `Bearer ${token}`)});
}
