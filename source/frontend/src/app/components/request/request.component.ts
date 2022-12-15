import {Component, Input, OnInit} from '@angular/core';
import {catchError, EMPTY, Observable, single} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {FormControl, FormGroup} from '@angular/forms';
import {map} from 'rxjs/operators';
import {NgbNavChangeEvent} from "@ng-bootstrap/ng-bootstrap";
import {IconNamesEnum} from "ngx-bootstrap-icons";

export type RequestData = { [key: string]: string; };

@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.scss']
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
