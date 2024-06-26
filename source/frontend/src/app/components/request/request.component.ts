import {Component, Input, OnInit} from '@angular/core';
import {catchError, EMPTY, map, Observable, single} from "rxjs";
import {FormControl, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {NgbModule, NgbNavChangeEvent} from "@ng-bootstrap/ng-bootstrap";
import {NgxJsonViewerModule} from "ngx-json-viewer";
import {ClipboardModule} from "ngx-clipboard";
import {JsonPipe, NgOptimizedImage} from "@angular/common";

export type RequestData = { [key: string]: string; };

@Component({
  selector: 'app-request',
  standalone: true,
  imports: [ReactiveFormsModule, NgxJsonViewerModule, NgbModule, ClipboardModule, JsonPipe, NgOptimizedImage],
  templateUrl: './request.component.html',
  styleUrl: './request.component.scss',
})
export class RequestComponent implements OnInit {
  @Input() public title?: string;
  @Input() public protection?: 'anonymous' | 'authenticated' | 'authorized';
  @Input() public request?: (requestData: RequestData) => Observable<any>;
  @Input() public requestData?: RequestData;

  public requestDataForm?: FormGroup;
  public response: any;

  public ngOnInit(): void {
    this.requestDataForm = this.createRequestDataForm();
  }

  public senRequest(): void {
    if (this.request === undefined)
      return;

    const requestData = this.requestDataForm?.value;
    this.request(requestData)
      .pipe(
        single(),
        catchError(error => {
          this.response = {status: error.status, statusText: error.statusText, message: error.error};
          return EMPTY;
        }),
        map(response => response ?? 'OK')
      )
      .subscribe(response => this.response = response);
  }

  public getFormControlNames(): string[] {
    if (this.requestData === undefined)
      return [];
    return Object.keys(this.requestData);
  }

  public onNavChange(changeEvent: NgbNavChangeEvent) {

    if (changeEvent.nextId === 'copy') {
      changeEvent.preventDefault();
    }
  }

  private createRequestDataForm(): FormGroup {
    const requestDataForm = new FormGroup({});
    if (this.requestData === undefined)
      return requestDataForm;

    const formControlEntries = Object.entries(this.requestData);
    for (const [key, value] of formControlEntries)
      requestDataForm.addControl(key, new FormControl(value));

    return requestDataForm;
  }
}
