import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Activity } from '../interfaces/activity';
import { AdresCompleteService } from '../services/adres-complete.service';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { ErrorMessagesService } from '../services/error-messages.service';
import { ActivitiesService } from '../services/activities.service';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.css']
})
export class EventFormComponent implements OnInit {

  activityForm: FormGroup;
  filteredOptions: Observable<any[]> = {} as Observable<any[]>;

  constructor(private fb: FormBuilder, private activityService: ActivitiesService, public errorMessageService: ErrorMessagesService) {
    this.activityForm = this.fb.group({
      description: ['', Validators.required],
      date: ['', Validators.required],
      address: this.fb.group({
        houseNumber: ['', Validators.required],
        road: [''],
        state: ['', Validators.required],
        postcode: ['', [Validators.required, Validators.pattern('^[0-9]{2}-[0-9]{3}$')]],
        city: ['', Validators.required],
      }),
    });
  }

  ngOnInit(): void {

  }

  onSubmit(): void {

    console.log('New Product:', this.activityForm.value);

    if (this.activityForm.valid) {
      const newActivity: Activity = this.activityForm.value;
      console.log('New Product:', newActivity);

      this.activityService.addActivity(newActivity);
    }
  }

}
