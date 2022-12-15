import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {NgxBootstrapIconsModule, shield, shieldShaded, shieldFill, clipboardCheck} from 'ngx-bootstrap-icons';

import {AppRoutingModule} from './app-routing.module';
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
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
