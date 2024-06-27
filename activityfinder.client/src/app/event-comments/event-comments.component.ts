import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Activity } from '../interfaces/activity';
import { ActivitiesService } from '../services/activities.service';
import { AuthenticateService } from '../services/authenticate.service';
import { CommentService, Comment } from '../services/comment.service';

@Component({
  selector: 'app-event-comments',
  templateUrl: './event-comments.component.html',
  styleUrls: ['./event-comments.component.css']
})
export class EventCommentsComponent implements OnInit, OnDestroy {

  id: string = '';
  comments$!: Observable<Comment[]>;
  activity$!: Observable<Activity>;
  userName = '';

  commentInput: string = '';

  constructor(private route: ActivatedRoute, private commentService: CommentService, private activityService: ActivitiesService,
    private authSerive: AuthenticateService) { }   

  async ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id')!;
    this.commentService.joinEvent(this.id);
    this.comments$ = this.commentService.getMessages(+this.id);
    this.activity$ = this.activityService.getActivity(this.id);
    this.userName = this.authSerive.getUsername() || '';
  }

  addComment() {
    this.commentService.createComment(this.commentInput, +this.id);
    this.commentInput = '';
  }

  edit() {
    console.log('edit')
    console.log(this.userName);
  }

  delete() {

  }

  ngOnDestroy(): void {
    this.commentService.leaveEvent(this.id);
  }

}
