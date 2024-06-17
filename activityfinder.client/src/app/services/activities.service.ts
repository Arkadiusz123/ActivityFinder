import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Activity, ActivityListItem } from '../interfaces/activity';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ActivitiesService {

 // private objectsSubject = new BehaviorSubject<Activity[]>([]);   todo test adding

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

  joinActivity(id: number): Observable<any> {
    return this.http.post<Activity>('/api/activity/join/' + id, null);
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
}
