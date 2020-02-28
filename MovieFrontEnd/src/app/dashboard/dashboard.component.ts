import { UserService } from '../services/user.service';
import { DashboardService } from '../services/dashboard.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";
import { AppUser } from 'src/models';
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
  notUser: string;

  enter: boolean;

  users: AppUser[];

  constructor(private dashboardService: DashboardService, private router: Router, private route: ActivatedRoute, private userService: UserService, private toastr: ToastrService, private spinner: NgxSpinnerModule) {
    this.route.url.subscribe(() => {
      const category = route.snapshot.firstChild.data.breadcrumb;
      this.selectCategory(category);
    });

    const me = this;
    this.route.params.subscribe(params => {
      me.notUser = params.id;
    });
  }

  ngOnInit() {
    this.getUser();
  }

  getUser() {
    const me = this;
    this.userService.currentUser.subscribe(result => {
      me.currentUser = result;
    },
      err => {
        console.log(err);
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
    this.enter = true;
  }

  onChangeSearch(val: string) {
    if (this.enter) {
      this.router.navigateByUrl('/dashboard/profile/' + val);
    } else {
      this.enter = false;
    }
  }

  onFocused(e) {
    this.grabAllUsers();
  }
}

