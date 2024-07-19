import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { AuthenticateService } from './authenticate.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private commentsSubject: BehaviorSubject<Comment[]> = new BehaviorSubject<Comment[]>([]);
  private hubConnection!: signalR.HubConnection;

  constructor(private http: HttpClient, private authService: AuthenticateService) {
    const token = this.authService.getToken();
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(`${environment.backendUrl}/commentHub`,
      {
        accessTokenFactory: () => token ?? '',
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      }
    ).configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection.on('ReceiveComment', (comment: Comment) => this.addCommentToSub(comment));
    this.hubConnection.on('DeleteComment', (id: number) => this.removeCommentFromSub(id));
    this.hubConnection.on('EditComment', (comment: Comment) => this.editCommentInSub(comment));
  }

  getMessages(id: number): Observable<Comment[]> {
    this.http.get<Comment[]>(`${environment.backendUrl}/comment/activity/` + id).subscribe(x => this.commentsSubject.next(x));
    return this.commentsSubject.asObservable();
  }

  createComment(content: string, id: number) {
    this.http.post(`${environment.backendUrl}/comment/activity/` + id, { content: content }).subscribe();
  }

  editComment(comment: Comment) {
    this.http.put(`${environment.backendUrl}/comment/`, comment).subscribe();
  }

  deleteComment(id: number) {
    this.http.delete(`${environment.backendUrl}/comment/` + id).subscribe();
  }

  joinEvent(eventId: string) {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('JoinEventGroup', eventId);
    }
    else {
      this.hubConnection.start().then(() => this.hubConnection.invoke('JoinEventGroup', eventId));
    }        
  }

  leaveEvent(eventId: string) {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('LeaveEventGroup', eventId).catch(err => console.error(err));
    }    
  }

  closeConnection() {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.stop();
    }   
  }

  private addCommentToSub(comment: Comment): void {
    const currentComments = this.commentsSubject.getValue();
    this.commentsSubject.next([...currentComments, comment]);
  }

  private editCommentInSub(comment: Comment): void {
    const currentComments = this.commentsSubject.getValue();
    const foundIndex = currentComments.findIndex(x => x.id == comment.id);
    currentComments[foundIndex] = comment;

    this.commentsSubject.next(currentComments);
  }

  private removeCommentFromSub(id: number): void {
    const currentComments = this.commentsSubject.getValue();
    this.commentsSubject.next(currentComments.filter(x => x.id !== id));
  }
}

export interface Comment {
  id: number;
  content: string;
  userName: string;
  date: Date;
}
