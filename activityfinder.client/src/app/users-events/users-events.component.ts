import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ActivityListItem } from '../interfaces/activity';
import { ActivitiesService } from '../services/activities.service';

@Component({
  selector: 'app-users-events',
  templateUrl: './users-events.component.html',
  styleUrls: ['./users-events.component.css']
})
export class UsersEventsComponent implements OnInit {
  activities!: Observable<ActivityListItem[]>;

  constructor(private activityService: ActivitiesService) { }

  ngOnInit(): void {
    this.activities = this.activityService.getUsersActivities();
  }

}
