import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { Activity } from '../interfaces/activity';
import { ErrorMessagesService } from '../services/error-messages.service';
import { ActivitiesService } from '../services/activities.service';
import { AddressFinderService } from '../services/address-finder.service';
import Swal from 'sweetalert2';

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
      title: ['', Validators.required],
      date: ['', Validators.required],
      otherInfo: [''],
      address: this.fb.group({
        osmId: ['']
      }),
    });
  }

  ngOnInit(): void {

  }

  findAddress(event: KeyboardEvent | null) {
    event?.preventDefault();

    if (!this.searchInput) {
      return;
    }

    const nameControl = this.activityForm.get('address.displayName');
    if (nameControl && nameControl.value) {
      Swal.fire('Usuń wybrany adres, aby wyszukać inny.');
      return;
    }

    var addressSubscription = this.addressFinder.findAddress(this.searchInput).subscribe(res => {
      if (res.length == 0) {
        Swal.fire('Nie znaleziono adresu')
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
      Swal.fire('Nie wybrano adresu')
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
