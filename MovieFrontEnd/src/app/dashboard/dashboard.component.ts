import { UserService } from '../services/user.service';
import { DashboardService } from '../services/dashboard.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";
import { AppUser } from 'src/models';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styles: []
})
export class DashboardComponent implements OnInit {

  currentUser: AppUser;
  push: boolean = false;

  constructor(private router: Router, private userService: UserService, private dashboardService: DashboardService, private toastr: ToastrService, private spinner: NgxSpinnerModule) { }

  ngOnInit() {
    const me = this;
    this.userService.currentUser.subscribe(result => {
      me.currentUser = result;
      console.log(me.currentUser)
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
}
