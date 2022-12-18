import {APP_INITIALIZER, NgModule} from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {AppRoutingModule} from './app-routing.module';
import {BrowserModule} from '@angular/platform-browser';
import {Observable} from 'rxjs';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {NgxBootstrapIconsModule, shield, shieldShaded, shieldFill, clipboardCheck} from 'ngx-bootstrap-icons';

import {AuthenticationInterceptor} from './services/authentication.interceptor';
import {AuthenticationService} from './services/authentication.service';
import {AppComponent} from './app.component';
import {RequestComponent} from './components/request/request.component';
import {ReactiveFormsModule} from '@angular/forms';
import {NgxJsonViewerModule} from "ngx-json-viewer";
import {ClipboardModule} from "ngx-clipboard";

const bootstrapIcons = {
  shield,
  shieldShaded,
  shieldFill,
  clipboardCheck,
};

@NgModule({
  declarations: [
    AppComponent,
    RequestComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    NgxBootstrapIconsModule.pick(bootstrapIcons),
    NgxJsonViewerModule,
    ClipboardModule,
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: keycloakLoaderFactory,
      deps: [AuthenticationService],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}

export function keycloakLoaderFactory(authenticationService: AuthenticationService): () => Promise<any> | Observable<any> {
  return () => authenticationService.init();
}
