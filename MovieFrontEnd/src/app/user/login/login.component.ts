import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {
  formModel = {
    UserName: '',
    Password: ''
  }
  constructor(private userService: UserService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
    if (localStorage.getItem('token') != null)
      this.router.navigateByUrl('/dashboard/profile/' + this.formModel.UserName);
  }

  onSubmit(form: NgForm) {
    const me = this;
    this.userService.login(form.value).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
        me.router.navigateByUrl('/dashboard/profile/' + me.formModel.UserName);
        me.toastr.success("Welcome, " + me.formModel.UserName);
      },
      err => {
        me.toastr.error('Incorrect username or password.', 'Login failed');
        
      }
    );
  }
}
