import { UserService } from '../services/user.service';
import { DashboardService } from '../services/dashboard.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";
import { AppUser, Notifications, Friends } from 'src/models';
import { AutocompleteLibModule } from 'angular-ng-autocomplete';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styles: []
})
export class DashboardComponent implements OnInit {

  keyword = 'userName';
  currentUser: AppUser = {};
  activeCategory: string;
  notifications: Notifications[];
  friends: Friends[];
  friendRequests: number;
  messages: number;
  resetNotification: boolean;

  enter: boolean;

  users: AppUser[];

  constructor(private dashboardService: DashboardService, private router: Router, private route: ActivatedRoute, private userService: UserService, private toastr: ToastrService) {
    this.route.url.subscribe(() => {
      const category = route.snapshot.firstChild.data.breadcrumb;
      this.selectCategory(category);
    });
  }

  ngOnInit() {
    this.enter = false;
    this.resetNotification = false;
    this.getUser();
  }

  getUser() {
    const me = this;
    this.userService.currentUser.subscribe(result => {
      me.currentUser = result;
      me.getNotifications();
      me.getFriendRequests();
    },
      err => {
        console.log(err);
      },
    );
  }

  getNotifications() {
    const me = this;
    this.dashboardService.getNotifications(this.currentUser.id).subscribe(result => {
      me.notifications = result;
    },
      err => {
        console.log(err);
      },
    );
  }

  getFriendRequests() {
    const me = this;
    this.dashboardService.getFriendRequests(this.currentUser.id, this.resetNotification).subscribe(result => {
      me.friends = result;

      if (me.resetNotification) {
        me.resetNotification = false;
        me.getNotifications();
      }
    },
      err => {
        console.log(err);
      },
    );
  }

  acceptFriendRequest(friendId: number) {
    const me = this;
    this.dashboardService.acceptFriendRequest(this.currentUser.id, friendId).subscribe(data => {
      me.toastr.success("Friend Request Accepted");
      me.getFriendRequests();
    },
      err => {
        console.log(err);
        me.toastr.error("Friend Request Could Not Be Updated");
      },
    );
  }

  grabAllUsers() {
    const me = this;
    this.dashboardService.grabAllUsers().subscribe(result => {
      me.users = result;
    },
      err => {
        console.log(err);
      },
    );
  }

  onLogout() {
    this.toastr.success('Succssfully Logged Out');
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }

  selectCategory(newValue) {
    this.activeCategory = newValue;
  }

  selectEvent(item) {
    this.router.navigateByUrl('/dashboard/profile/' + item.userName);
  }

  onEnter(event) {
    if (event) {
      this.enter = true;
    }
  }

  onChangeSearch(val: string) {
    if (this.enter) {
      this.router.navigateByUrl('/dashboard/profile/' + val);
      this.enter = false;
      return;
    }
  }

  onFocused(e) {
    this.grabAllUsers();
  }
}

