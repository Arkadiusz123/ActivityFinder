import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Activity, ActivityListItem } from '../interfaces/activity';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ActivitiesService {

  constructor(private http: HttpClient, private router: Router) { }

  addActivity(activity: Activity) {
    this.http.post<Activity>(`${environment.backendUrl}/activity`, activity).subscribe(res => {
        this.router.navigate([''])
      });
  }

  editActivity(activity: Activity, id: number) {
    activity.id = id;
    this.http.put<Activity>(`${environment.backendUrl}/activity`, activity).subscribe(res => {
      this.router.navigate([''])
    });
  }

  getActivity(id: string): Observable<Activity> {
    return this.http.get<Activity>(`${environment.backendUrl}/activity/` + id);
  }

  activitiesList(settings: ActivitiesPaginationSettings): Observable<{ data: ActivityListItem[], totalCount: number }>
  {
    let params = new URLSearchParams();
    for (let key in settings) {
      params.set(key, settings[key])
    }

    return this.http.get<{ data: ActivityListItem[], totalCount: number }>(
      `${environment.backendUrl}/activity?${params.toString()}`
    );
  }

  getUsersActivities(): Observable<ActivityListItem[]> {
    return this.http.get<ActivityListItem[]>(`${environment.backendUrl}/activity/user`);
  }

  joinActivity(id: number): Observable<any> {
    return this.http.post<Activity>(`${environment.backendUrl}/activity/join/` + id, null);
  }

  leaveActivity(id: number): Observable<any> {
    return this.http.post<Activity>(`${environment.backendUrl}/activity/leave/` + id, null);
  }

  deleteActivity(id: number): Observable<any> {
    return this.http.delete<Activity>(`${environment.backendUrl}/activity/` + id);
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
