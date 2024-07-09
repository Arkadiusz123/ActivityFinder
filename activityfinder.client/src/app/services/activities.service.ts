import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Activity, ActivityListItem } from '../interfaces/activity';

@Injectable({
  providedIn: 'root'
})
export class ActivitiesService {

  constructor(private http: HttpClient, private router: Router) { }

  addActivity(activity: Activity) {
    this.http.post<Activity>('/api/activity', activity).subscribe(res => {
        this.router.navigate([''])
      });
  }

  editActivity(activity: Activity, id: number) {
    activity.id = id;
    this.http.put<Activity>('/api/activity', activity).subscribe(res => {
      this.router.navigate([''])
    });
  }

  getActivity(id: string): Observable<Activity> {
    return this.http.get<Activity>('/api/activity/' + id);
  }

  activitiesList(settings: ActivitiesPaginationSettings): Observable<{ data: ActivityListItem[], totalCount: number }>
  {
    let params = new URLSearchParams();
    for (let key in settings) {
      params.set(key, settings[key])
    }

    return this.http.get<{ data: ActivityListItem[], totalCount: number }>(
      `/api/activity?${params.toString()}`
    );
  }

  getUsersActivities(): Observable<ActivityListItem[]> {
    return this.http.get<ActivityListItem[]>('/api/activity/user');
  }

  joinActivity(id: number): Observable<any> {
    return this.http.post<Activity>('/api/activity/join/' + id, null);
  }

  leaveActivity(id: number): Observable<any> {
    return this.http.post<Activity>('/api/activity/leave/' + id, null);
  }

  deleteActivity(id: number): Observable<any> {
    return this.http.delete<Activity>('/api/activity/' + id);
  }
}

export interface ActivitiesPaginationSettings {
  page: number;
  size: number;
  sortField: string;
  asc: boolean;
  address: string;
  state: string;
  status: number;
  finished: boolean;
  full: boolean;
}
