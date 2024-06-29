import {Component} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {RequestComponent, RequestData} from "./components/request/request.component";
import {Observable} from "rxjs";
import {HttpClient, HttpParams} from "@angular/common/http";
import {AuthenticationService} from "./services/authentication.service";
import {routes} from "./services/routes";
import {environment} from '../environments/environment';
import {NgIf} from "@angular/common";
import {NgbNavModule} from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RequestComponent, NgIf, NgbNavModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  public environment = environment;
  public isAuthenticated: boolean;
  public username: string | undefined;

  public getArticleData: RequestData = {id: ''};
  public addArticleData: RequestData = {id: '', description: ''};
  public removeArticleData: RequestData = {id: ''};

  public getArticles: () => Observable<any[]>;
  public getArticle: (article: RequestData) => Observable<any>;
  public addArticle: (article: RequestData) => Observable<Object>;
  public removeArticle: (article: RequestData) => Observable<Object>;
  public getApplicationName: () => Observable<string>;
  public getCurrentUser: () => Observable<any>;

  get activeTab(): number {
    return Number.parseInt(localStorage.getItem('api_active_tab') ?? '1');
  }

  set activeTab(value: number) {
    localStorage.setItem('api_active_tab', value.toString());
  }

  constructor(
    private httpClient: HttpClient,
    private authenticationService: AuthenticationService,
  ) {
    this.isAuthenticated = this.authenticationService.isAuthenticated;
    this.username = this.authenticationService.username;

    this.getArticles = () => this.httpClient.get<any[]>(routes.article.getArticles);
    this.getArticle = (article) => this.httpClient.get<any>(`${routes.article.getArticle}?${new HttpParams().append('id', article['id'])}`);
    this.addArticle = (article) => this.httpClient.post(routes.article.addArticle, article,);
    this.removeArticle = (article) => this.httpClient.delete(`${routes.article.removeArticle}?${new HttpParams().append('id', article['id'])}`);
    this.getApplicationName = () => this.httpClient.get(routes.information.getApplicationName, {responseType: 'text'});
    this.getCurrentUser = () => this.httpClient.get(routes.user.getCurrentUser);
  }

  public login(): void {
    this.authenticationService.login();
  }

  public logout(): void {
    this.authenticationService.logout();
  }
}
