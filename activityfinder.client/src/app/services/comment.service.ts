import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  //private commentsSubject: BehaviorSubject<Comment[]> = new BehaviorSubject<Comment[]>([]);  

  constructor(private http: HttpClient) {
    
  }

  getMessages(id: number): Observable<Comment[]> {
    //if (this.commentsSubject.value.length === 0) {
    //  this.http.get<Comment[]>('/api/comment/activity/' + id).subscribe(x => this.commentsSubject.next(x));
    //}
    //return this.commentsSubject.asObservable();
    return this.http.get<Comment[]>('/api/comment/activity/' + id);
  }

  createComment(content: string, id: number) {
    this.http.post('/api/comment/activity/' + id, { content: content }).subscribe();
  }
}

export interface Comment {
  id: number;
  content: string;
  userName: string;
  date: Date;
}
