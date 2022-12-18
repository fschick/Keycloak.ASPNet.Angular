import {APP_INITIALIZER, ApplicationConfig, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {provideHttpClient, withInterceptors} from "@angular/common/http";
import {authenticationInterceptor} from "./services/authentication.interceptor";
import {AuthenticationService} from "./services/authentication.service";
import {Observable} from "rxjs";

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({eventCoalescing: true}),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authenticationInterceptor])),
    {
      provide: APP_INITIALIZER,
      useFactory: keycloakLoaderFactory,
      multi: true,
      deps: [AuthenticationService],
    },
  ]
};

export function keycloakLoaderFactory(authenticationService: AuthenticationService): () => Promise<any> | Observable<any> {
  return () => authenticationService.init();
}
