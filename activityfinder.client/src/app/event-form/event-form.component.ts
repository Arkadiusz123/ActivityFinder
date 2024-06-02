import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { Activity } from '../interfaces/activity';
import { AdresCompleteService } from '../services/adres-complete.service';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { ErrorMessagesService } from '../services/error-messages.service';
import { ActivitiesService } from '../services/activities.service';
import { AddressFinderService } from '../services/address-finder.service';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.css']
})
export class EventFormComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  activityForm: FormGroup;
  filteredOptions: Observable<any[]> = {} as Observable<any[]>;

  displayAddress: string = '';
  searchInput: string = '';

  constructor(private fb: FormBuilder, private activityService: ActivitiesService, private addressFinder: AddressFinderService,
    public errorMessageService: ErrorMessagesService) {
    this.activityForm = this.fb.group({
      description: ['', Validators.required],
      date: ['', Validators.required],
      address: this.fb.group({
        osmId: ['']
      }),
    });
  }

  ngOnInit(): void {

  }

  delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  async findAddress(event: KeyboardEvent | null) {
    event?.preventDefault();

    await this.delay(300);
    const nameControl = this.activityForm.get('address.displayName');
    if (nameControl && nameControl.value) {
      alert('Usuń wybrany adres, aby wyszukać inny.');
      return;
    }

    var addressSubscription = this.addressFinder.findAddress(this.searchInput).subscribe(res => {
      if (res.length == 0) {
        alert('Nie znaleziono adresu')
      }
      else {
        this.activityForm.get('address.osmId')!.setValue(res.osmId);
        this.displayAddress = res.displayName;
        this.searchInput = '';
      }
    })
    this.subscriptions.push(addressSubscription);
  }

  removeAddress() {
    this.activityForm.get('address.osmId')!.setValue('');
    this.displayAddress = '';
  }

  onSubmit() {
    //console.log('New Product:', this.activityForm.value);
    if (!this.activityForm.get('address.osmId')!.value) {
      alert('Nie wybrano adresu')
      return;
    }

    if (this.activityForm.valid) {
      const newActivity: Activity = this.activityForm.value;
      this.activityService.addActivity(newActivity);
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

}
