import {Component, Input, OnInit} from '@angular/core';
import {catchError, EMPTY, map, Observable, single} from "rxjs";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {NgbModule, NgbNavChangeEvent} from "@ng-bootstrap/ng-bootstrap";
import {NgxJsonViewerModule} from "ngx-json-viewer";
import {ClipboardModule} from "ngx-clipboard";
import {JsonPipe, NgOptimizedImage} from "@angular/common";
import {FormSubmitDirective} from "../../directives/form-submit.directive";

export type RequestData = { [key: string]: { required?: boolean }; };

@Component({
    selector: 'app-request',
    imports: [ReactiveFormsModule, NgxJsonViewerModule, NgbModule, ClipboardModule, JsonPipe, NgOptimizedImage, FormSubmitDirective],
    templateUrl: './request.component.html',
    styleUrl: './request.component.scss'
})
export class RequestComponent implements OnInit {
    @Input() public title?: string;
    @Input() public protection?: 'anonymous' | 'authenticated' | 'authorized';
    @Input() public request?: (requestData: RequestData) => Observable<any>;
    @Input() public requestData?: RequestData;
    @Input() public resultDepth?: number;

    public id = '';
    public requestDataForm?: FormGroup;
    public response: any;

    public ngOnInit(): void {
        this.id = this.hash(this.title);
        this.requestDataForm = this.createRequestDataForm();
    }

    public sendRequest(): void {
        if (this.request === undefined)
            return;

        if (!this.requestDataForm?.valid)
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
            requestDataForm.addControl(key, new FormControl('', value.required ? [Validators.required] : []));

        return requestDataForm;
    }

    private hash(value: string | undefined) {
        value = value ?? '';
        let hash = 0;
        for (let i = 0; i < value.length; i++) {
            const char = value.charCodeAt(i);
            hash = (hash << 5) - hash + char;
        }

        // Convert to 32bit unsigned integer in base 36 and pad with "0" to ensure length is 7.
        return (hash >>> 0).toString(36).padStart(7, '0');
    };
}
