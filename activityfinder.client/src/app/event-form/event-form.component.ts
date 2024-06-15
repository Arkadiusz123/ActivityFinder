import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { Activity } from '../interfaces/activity';
import { ErrorMessagesService } from '../services/error-messages.service';
import { ActivitiesService } from '../services/activities.service';
import { AddressFinderService } from '../services/address-finder.service';
import Swal from 'sweetalert2';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EventFormComponent implements OnInit, OnDestroy {
  activityForm: FormGroup;
  filteredOptions: Observable<any[]> = {} as Observable<any[]>;

  displayAddress: string = '';
  searchInput: string = '';
  id: string = '';

  private addressSub: Subscription | null = null;
  private activitySub: Subscription | null = null;

  constructor(private fb: FormBuilder, private activityService: ActivitiesService, private addressFinder: AddressFinderService,
    private changeDetectorRef: ChangeDetectorRef, private route: ActivatedRoute,
    public errorMessageService: ErrorMessagesService)
  {
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
    this.id = this.route.snapshot.paramMap.get('id') || '';

    if (this.id) {
      this.activitySub = this.activityService.getActivity(this.id).subscribe(x => {     
        this.activityForm.patchValue(x)
        this.displayAddress = x.address.displayName
      });
    }
  }

  findAddress(event: KeyboardEvent | null) {
    event?.preventDefault();

    if (this.addressSub !== null) {
      this.addressSub.unsubscribe();
    }

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
        this.changeDetectorRef.markForCheck();
      }
    })
    this.addressSub = addressSubscription;
  }

  removeAddress() {
    this.activityForm.get('address.osmId')!.setValue('');
    this.displayAddress = '';
  }

  onSubmit() {
    if (!this.activityForm.get('address.osmId')!.value) {
      Swal.fire('Nie wybrano adresu')
      return;
    }

    if (this.activityForm.valid) {
      const newActivity: Activity = this.activityForm.value;
      if (!this.id) {
        this.activityService.addActivity(newActivity);
      }
      else {
        this.activityService.editActivity(newActivity, +this.id);
      }     
    }
  }

  ngOnDestroy() {
    if (this.addressSub !== null) {
      this.addressSub.unsubscribe();
    }
    if (this.activitySub !== null) {
      this.activitySub.unsubscribe();
    }
  }

}
