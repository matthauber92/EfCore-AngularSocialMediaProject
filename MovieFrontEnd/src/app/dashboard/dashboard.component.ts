import { UserService } from '../services/user.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styles: []
})
export class DashboardComponent implements OnInit {

  currentUser;
  push: boolean = false;

  constructor(private router: Router, private service: UserService, private toastr: ToastrService, private spinner: NgxSpinnerModule) { }

  ngOnInit() {
    const me = this;
    this.service.getUserProfile().subscribe(
      res => {
        me.currentUser = res;
        me.toastr.success('Welcome, ' + this.currentUser.userName);
        console.log(this.currentUser);
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
