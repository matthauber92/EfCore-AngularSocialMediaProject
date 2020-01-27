import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from 'rxjs';
import { AppUser, Posts, Comments } from '../../models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getPosts(userID: number): Observable<Posts[]> {
    return this.http.get<Posts[]>(this.apiUrl + '/Dashboard/GetPosts?userId=' + userID);
  }
}
