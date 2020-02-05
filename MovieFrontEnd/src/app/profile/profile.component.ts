import { Component, OnInit, Input } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import { Posts } from 'src/models';
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  @Input() currentUser;
  userPosts: Posts[];
  newPost: Posts = {};

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {
    this.getPosts();
  }

  getPosts() {
    const me = this;
    this.dashboardService.getPosts(this.currentUser.id).subscribe(data => {
      me.userPosts = data;
    },
      err => {
        console.log(err);
      },
    );
  }

  onSubmit() {
    const me = this;
    this.dashboardService.submitPost(this.newPost, this.currentUser.id).subscribe(data => {
      me.newPost.content = "";
      me.getPosts();
    },
      err => {
        console.log(err);
      },
    );
  }

}
