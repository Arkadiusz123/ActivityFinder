import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Activity } from '../interfaces/activity';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ActivitiesService {

  private objectsSubject = new BehaviorSubject<Activity[]>([]);

  constructor(private http: HttpClient) { }

  addActivity(activity: Activity) {
    //this.http.get('/weatherforecast').subscribe();
    //return;

    this.http.post<Activity>('/api/activity', activity).pipe(
      tap(data => {
        const currentList = this.objectsSubject.value;
        currentList.push(data);
        this.objectsSubject.next(currentList);
      }))
      .subscribe();
  }
}
