import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from 'rxjs';
import { AppUser, Posts, Comments } from '../../models';
import { environment } from '../../environments/environment';
import { shareReplay, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private fb: FormBuilder, private http: HttpClient) { }
  readonly apiUrl = environment.apiUrl;
  private currentUser$: Observable<AppUser>;
  private cachedUser: AppUser;

    get currentUser() {
      if (!this.currentUser$) {
        this.currentUser$ = this.loadCurrentUser().pipe(
          shareReplay(1)
        );
      }

      return this.currentUser$;
    }

    private loadCurrentUser() {
      const me = this;
      return this.http.get<AppUser>(this.apiUrl + '/AppUser/GetUser').pipe(
        map(response => {
          me.cachedUser = response;
          return response;
        } )
      );
    }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    FirstName: [''],
    LastName: [""],
    Passwords: this.fb.group({
      Password: ['', [Validators.required, Validators.minLength(4)]],
      ConfirmPassword: ['', Validators.required]
    }, { validator: this.comparePasswords })

  });

  comparePasswords(fb: FormGroup) {
    let confirmPswrdCtrl = fb.get('ConfirmPassword');
    //passwordMismatch
    //confirmPswrdCtrl.errors={passwordMismatch:true}
    if (confirmPswrdCtrl.errors == null || 'passwordMismatch' in confirmPswrdCtrl.errors) {
      if (fb.get('Password').value != confirmPswrdCtrl.value)
        confirmPswrdCtrl.setErrors({ passwordMismatch: true });
      else
        confirmPswrdCtrl.setErrors(null);
    }
  }

  register() {
    var body = {
      UserName: this.formModel.value.UserName,
      Email: this.formModel.value.Email,
      FirstName: this.formModel.value.FirstName,
      LastName: this.formModel.value.LastName,
      Password: this.formModel.value.Passwords.Password
    };
    return this.http.post(this.apiUrl + '/AppUser/SignUp', body);
  }

  login(formData) {
    return this.http.post(this.apiUrl + '/AppUser/Login', formData);
  }
}
