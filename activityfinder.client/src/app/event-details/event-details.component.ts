import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Activity } from '../interfaces/activity';
import { ActivitiesService } from '../services/activities.service';
import { CommentService, Comment } from '../services/comment.service';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.css']
})
export class EventDetailsComponent implements OnInit {

  id: string = '';
  comments$!: Observable<Comment[]>;
  activity$!: Observable<Activity>;

  commentInput: string = '';

  constructor(private route: ActivatedRoute, private commentService: CommentService, private activityService: ActivitiesService) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id')!;
    this.comments$ = this.commentService.getMessages(+this.id);
    this.activity$ = this.activityService.getActivity(this.id);
  }

  addComment() {
    this.commentService.createComment(this.commentInput, +this.id);
    this.commentInput = '';
  }

}
