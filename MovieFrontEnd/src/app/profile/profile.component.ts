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

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {
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

}
