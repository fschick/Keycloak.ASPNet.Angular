import {Directive, ElementRef, OnDestroy, OnInit, Renderer2} from '@angular/core';
import {Subscription} from 'rxjs';
import {FormGroupDirective} from '@angular/forms';
import {filter} from 'rxjs/operators';

@Directive({
  selector: 'form',
  standalone: true
})
export class FormSubmitDirective implements OnInit, OnDestroy {
  private subscriptions = new Subscription();

  constructor(
    private hostElement: ElementRef,
    private renderer: Renderer2,
    private ngForm: FormGroupDirective,
  ) {
  }

  ngOnInit(): void {
    const ngSubmit = this.ngForm.ngSubmit.subscribe(() =>
      this.renderer.addClass(this.hostElement.nativeElement, 'ng-submitted')
    );

    const ngReset = this.ngForm.statusChanges
      ?.pipe(filter(() => this.ngForm?.submitted !== true))
      .subscribe(() =>
        this.renderer.removeClass(this.hostElement.nativeElement, 'ng-submitted')
      );

    this.subscriptions.add(ngSubmit);
    this.subscriptions.add(ngReset);
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
