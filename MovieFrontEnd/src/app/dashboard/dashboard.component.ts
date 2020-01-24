import { UserService } from '../shared/user.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styles: []
})
export class DashboardComponent implements OnInit {

  currentUser;
  push: boolean = false;

  constructor(private router: Router, private service: UserService, private toastr: ToastrService) { }

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
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }

  openNav() {
    document.getElementById("mySidenav").style.width = "250px";
    this.push = true;
  }
  
  closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    this.push = false;
  }
}
