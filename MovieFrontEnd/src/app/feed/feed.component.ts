import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import { Posts } from 'src/models';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit {

  feed: Posts[];

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {
    this.getFeed();
  }

  getFeed() {
    const me = this;
    this.dashboardService.listAllPosts().subscribe(data => {
      me.feed = data;
    },
      err => {
        console.log(err);
      },
    );
  }
}
