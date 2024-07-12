import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Activity } from '../interfaces/activity';
import { ActivitiesService } from '../services/activities.service';
import { AuthenticateService } from '../services/authenticate.service';
import { CommentService, Comment } from '../services/comment.service';

@Component({
  selector: 'app-event-comments',
  templateUrl: './event-comments.component.html',
  styleUrls: ['./event-comments.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EventCommentsComponent implements OnInit, OnDestroy {

  id: string = '';
  comments$!: Observable<Comment[]>;
  activity$!: Observable<Activity>;
  userName = '';

  commentInput: string = '';
  commentEditInput: string = '';
  editModeId: number | null = null;

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
    if (!this.commentInput) {
      return;
    }
    this.commentService.createComment(this.commentInput, +this.id);
    this.commentInput = '';
  }

  setEditMode(id: number, content: string) {
    if (!this.commentEditInput) {
      return;
    }
    this.editModeId = id;
    this.commentEditInput = content;
  }

  cancelEditMode() {
    this.editModeId = null;
    this.commentEditInput = '';
  }

  confirmEdit() {
    const comment = { id: this.editModeId!, content: this.commentEditInput } as Comment;
    this.cancelEditMode();
    this.commentService.editComment(comment);
  }

  delete(id: number) {
    this.commentService.deleteComment(id);
  }

  ngOnDestroy(): void {
    this.commentService.leaveEvent(this.id);
  }

}
