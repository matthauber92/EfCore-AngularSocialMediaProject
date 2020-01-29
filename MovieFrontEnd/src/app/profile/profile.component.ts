import { Component, OnInit, Input } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  @Input() currentUser;

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {
    console.log("user: " + this.currentUser.id);
    this.dashboardService.getPosts(this.currentUser.id).subscribe(data => {
      console.log(data);
    },
      err => {
        console.log(err);
      },
    );
  }

}
