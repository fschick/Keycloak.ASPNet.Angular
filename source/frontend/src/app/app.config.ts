import {ApplicationConfig, inject, provideAppInitializer, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {AuthenticationService} from "./services/authentication.service";
import {Observable} from "rxjs";
import {provideHttpClient, withInterceptors} from "@angular/common/http";
import {authenticationInterceptor} from "./services/authentication.interceptor";

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({eventCoalescing: true}),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authenticationInterceptor])),
    provideAppInitializer(() => {
      const initializerFn = (authLoaderFactory)(inject(AuthenticationService));
      return initializerFn();
    }),
  ]
};

export function authLoaderFactory(authenticationService: AuthenticationService): () => Promise<any> | Observable<any> {
  return () => authenticationService.init();
}
