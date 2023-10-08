```typescript
import {Component} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {catchError, EMPTY, single} from "rxjs";
import {JsonPipe} from "@angular/common";
import {AuthService} from "./app.config";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, JsonPipe],
  template:`
    <div><button (click)="login()">Log in</button></div>
    <div><button (click)="logout()">Logout</button></div>
    <div><button (click)="sendRequest()">Send Request</button></div>
    <div>Result:</div>
    <div>{{ requestResult }}</div>`,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'asp-keycloack-workshop';

  public requestResult?: string;

  constructor(
    private httpClient: HttpClient,
    private authService: AuthService,
  ) {
  }

  login() {
    this.authService.login();
  }

  logout() {
    this.authService.logout();
  }

  sendRequest() {
    this.httpClient.get('http://localhost:5000/WeatherForecast')
      .pipe(
        single(),
        catchError(this.handleError.bind(this))
      )
      .subscribe(forecast => {
        this.requestResult = JSON.stringify(forecast);
      });
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 0)
      this.requestResult = `A netowrk related error occurred: ${JSON.stringify(error.message)}`;
    else
      this.requestResult = `Status code ${error.status}, body: ${JSON.stringify(error.message)}`;

    return EMPTY;
  }
}
```