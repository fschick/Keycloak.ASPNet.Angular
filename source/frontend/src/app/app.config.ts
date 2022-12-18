import { ApplicationConfig, provideZoneChangeDetection, inject, provideAppInitializer } from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {provideHttpClient, withInterceptors} from "@angular/common/http";
import {authenticationInterceptor} from "./services/authentication.interceptor";
import {provideOAuthClient} from "angular-oauth2-oidc";
import {AuthenticationService} from "./services/authentication.service";
import {Observable} from "rxjs";

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({eventCoalescing: true}),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authenticationInterceptor])),
    provideOAuthClient(),
    provideAppInitializer(() => {
      const initializerFn = (authLoaderFactory)(inject(AuthenticationService));
      return initializerFn();
    }),
  ]
};

export function authLoaderFactory(authenticationService: AuthenticationService): () => Promise<any> | Observable<any> {
  return () => authenticationService.init();
}
