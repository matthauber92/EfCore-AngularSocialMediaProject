import { UserService } from '../services/user.service';
import { DashboardService } from '../services/dashboard.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";
import { AppUser } from 'src/models';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styles: []
})
export class DashboardComponent implements OnInit {

  searchedUser: string;
  currentUser: AppUser = {};
  activeCategory: string;

  constructor(private router: Router, private route: ActivatedRoute, private userService: UserService, private toastr: ToastrService, private spinner: NgxSpinnerModule) {
    this.route.url.subscribe(() => {
      const category = route.snapshot.firstChild.data.breadcrumb;
      this.selectCategory(category);
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

  onLogout() {
    this.toastr.success('Succssfully Logged Out');
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }

  userSearch() {
    this.router.navigateByUrl('/dashboard/profile/' + this.searchedUser);
  }

  selectCategory(newValue) {
    this.activeCategory = newValue;
  }
}

