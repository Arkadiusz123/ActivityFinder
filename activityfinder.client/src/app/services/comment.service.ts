import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { AuthenticateService } from './authenticate.service';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private commentsSubject: BehaviorSubject<Comment[]> = new BehaviorSubject<Comment[]>([]);
  private hubConnection!: signalR.HubConnection;

  constructor(private http: HttpClient, private authService: AuthenticateService) {
    //const token = this.authService.getToken();
    //this.hubConnection = new signalR.HubConnectionBuilder().withUrl(`api/commentHub`,
    //  {
    //    accessTokenFactory: () => token ?? ''
    //  }
    //).build();
    //this.hubConnection.on('ReceiveComment', (comment: Comment) => this.addCommentToSub(comment));
  }

  getMessages(id: number): Observable<Comment[]> {
    this.http.get<Comment[]>('/api/comment/activity/' + id).subscribe(x => this.commentsSubject.next(x));
    //return this.http.get<Comment[]>('/api/comment/activity/' + id);
    return this.commentsSubject.asObservable();
  }

  createComment(content: string, id: number) {
    this.http.post('/api/comment/activity/' + id, { content: content }).subscribe();
  }

  joinEvent(eventId: string) {
    const token = this.authService.getToken();
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(`api/commentHub`,
      {
        accessTokenFactory: () => token ?? '',
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      }
    ).configureLogging(signalR.LogLevel.Information)
      .build();

    //await this.hubConnection.start();
    this.hubConnection.start().then(() => this.hubConnection.invoke('JoinEventGroup', eventId));
    //try {
    //  this.hubConnection.invoke('JoinEventGroup', eventId);
    //  console.log('joined group')
    //} catch (e) {
    //  console.log(e)
    //}
    
    this.hubConnection.on('ReceiveComment', (comment: Comment) => this.addCommentToSub(comment));
  }

  leaveEvent(eventId: string) {
    //this.hubConnection.invoke('LeaveEventGroup', eventId).catch(err => console.error(err));
     this.hubConnection.stop();
  }

  private addCommentToSub(comment: Comment): void {
    console.log('new comment')
    const currentComments = this.commentsSubject.getValue();
    this.commentsSubject.next([...currentComments, comment]);
  }
}

export interface Comment {
  id: number;
  content: string;
  userName: string;
  date: Date;
}
