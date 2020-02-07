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

  getPosts(userId: number): Observable<Posts[]> {
    return this.http.get<Posts[]>(this.apiUrl + '/Dashboard/GetPosts?userId=' + userId);
  }

  submitPost(post: Posts, userId: number): Observable<Posts> {
    const data = post;
    return this.http.post<Posts>(this.apiUrl + '/Dashboard/SubmitPost?userId=' + userId, data);
  }

  updateBio(userId: number, bio: string): Observable<string> {
    return this.http.post<string>(this.apiUrl + '/Dashboard/UpdateBio?userId=' + userId, bio);
  }

  deletePost(postId: number): Observable<boolean> {
    return this.http.delete<boolean>(this.apiUrl + '/Dashboard/' + postId);
  }
}
