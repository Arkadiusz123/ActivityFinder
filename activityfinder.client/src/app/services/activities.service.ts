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
    this.http.post<Activity>('/api/activity', activity)
      //.pipe(
      //tap(data => {
      //  const currentList = this.objectsSubject.value;
      //  currentList.push(data);
      //  this.objectsSubject.next(currentList);
      //}))
      .subscribe(res => {
        this.router.navigate([''])
      });
  }

  activitiesList(pageIndex: number, pageSize: number, sortField: string, sortDirection: string, filter: string, state: string):
    Observable<{ data: ActivityListItem[], totalCount: number }>
  {
    return this.http.get<{ data: ActivityListItem[], totalCount: number }>(
      `/api/activity?page=${pageIndex + 1}&size=${pageSize}&sortField=${sortField || 'date'}&asc=${sortDirection == 'asc'}&filter=${filter}&state=${state}`
    );
  }
}
