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
    this.newPost.Content = "test";
    console.log(this.newPost);
    const me = this;
    console.log("user: " + this.currentUser.id);
    this.dashboardService.getPosts(this.currentUser.id).subscribe(data => {
      console.log(data);
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
      console.log(data);
    },
      err => {
        console.log(err);
      },
    );
  }

}
