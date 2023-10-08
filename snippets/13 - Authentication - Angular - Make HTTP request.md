```typescript
public makeRequest(): void {
  this.httpClient
    .get('https://localhost:7028/WeatherForecast')
    .pipe(
      single(),
      catchError(this.handleError.bind(this))
    )
    .subscribe(result => {
      this.requestResult = JSON.stringify(result);
    });
}

private handleError(error: HttpErrorResponse) {
  if (error.status === 0)
    this.requestResult = `A netowrk related error occurred: ${JSON.stringify(error.message)}`;
  else
    this.requestResult = `Status code ${error.status}, body: ${JSON.stringify(error.message}`;

  return EMPTY;
}
```