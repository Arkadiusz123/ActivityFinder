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

  manualAddress: boolean = false;
  searchInput: string = '';

  constructor(private fb: FormBuilder, private activityService: ActivitiesService, private addressFinder: AddressFinderService,
    public errorMessageService: ErrorMessagesService) {
    this.activityForm = this.fb.group({
      description: ['', Validators.required],
      date: ['', Validators.required],
      address: this.fb.group({
        houseNumber: [''],
        road: [''],
        state: [''],
        postcode: [''],
        city: [''],
        displayName: [''],
      }),
    });
  }

  ngOnInit(): void {

  }

  manualAddressClick() {
    this.addValidators();
    this.removeValidators();
    this.updateValidators();
  }

  removeValidators() {
    if (!this.manualAddress) {
      this.activityForm.get('address.houseNumber')!.clearValidators();
      this.activityForm.get('address.state')!.clearValidators();
      this.activityForm.get('address.postcode')!.clearValidators();
      this.activityForm.get('address.city')!.clearValidators();
    }
    else {
      this.activityForm.get('address.displayName')!.clearValidators();
    }
  }

  addValidators() {
    if (!this.manualAddress) {
      this.activityForm.get('address.displayName')!.setValidators(Validators.required);
    }
    else {
      this.activityForm.get('address.houseNumber')!.setValidators(Validators.required);
      this.activityForm.get('address.state')!.setValidators(Validators.required);
      this.activityForm.get('address.postcode')!.setValidators([Validators.required, Validators.pattern('^[0-9]{2}-[0-9]{3}$')]);
      this.activityForm.get('address.city')!.setValidators(Validators.required);
    }
  }

  updateValidators() {
    this.activityForm.get('address.houseNumber')!.updateValueAndValidity();
    this.activityForm.get('address.state')!.updateValueAndValidity();
    this.activityForm.get('address.postcode')!.updateValueAndValidity();
    this.activityForm.get('address.city')!.updateValueAndValidity();
    this.activityForm.get('address.displayName')!.updateValueAndValidity();
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
        this.activityForm.get('address.displayName')!.setValue(res[0].display_name);
      }     
    })

    this.subscriptions.push(addressSubscription);
  }

  onSubmit(): void {
    //console.log('New Product:', this.activityForm.value);

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
