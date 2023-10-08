```typescript
import {Component, inject} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {AuthServiceService} from "./app.config";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {catchError, EMPTY, single} from "rxjs";

@Component({
    selector: 'app-root',
    imports: [RouterOutlet],
    template: `
        <h1>Welcome to {{ title }}!</h1>

        <div><button (click)="login()">Log in</button></div>
        <div><button (click)="logout()">Logout</button></div>
        <div><button (click)="sendRequest()">Send Request</button></div>
        <div>Result:</div>
        <div>{{ requestResult }}</div>

        <router-outlet/>
    `,
    styles: [],
})
export class AppComponent {
    private httpClient = inject(HttpClient);
    private authService = inject(AuthServiceService);
    protected requestResult?: string;
	// ...

    protected login() {
        this.authService.login();
    }

    protected logout() {
        this.authService.logout();
    }

    protected sendRequest() {
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