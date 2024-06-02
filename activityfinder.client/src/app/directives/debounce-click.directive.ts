import {
  Directive,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ElementRef,
  Renderer2
} from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { throttleTime } from 'rxjs/operators';

@Directive({
  selector: '[appDebounceClick]'
})
export class DebounceClickDirective implements OnInit, OnDestroy {
  @Input()
  debounceTime = 1000;

  @Input()
  eventName = 'click';

  @Output()
  debounceClickOrPress = new EventEmitter();

  private eventUnlistener!: () => void;

  private clicks = new Subject();
  private subscription!: Subscription;

  constructor(private elementRef: ElementRef, private renderer: Renderer2) { }

  ngOnInit() {
    this.subscription = this.clicks.pipe(
      throttleTime(this.debounceTime)
    ).subscribe(e => this.debounceClickOrPress.emit(e));

    this.addEventListener();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
    this.removeEventListener();
  }

  private addEventListener(): void {
    this.eventUnlistener = this.renderer.listen(this.elementRef.nativeElement, this.eventName, (event) => this.clickEvent(event));
  }

  private removeEventListener(): void {
    if (this.eventUnlistener) {
      this.eventUnlistener();
    }
  }

  clickEvent(event: any) {
    event.preventDefault();
    event.stopPropagation();
    this.clicks.next(event);
  }
}
