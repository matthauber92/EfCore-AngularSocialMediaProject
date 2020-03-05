import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from 'rxjs';
import { AppUser, Posts, Comments, Friends, Notifications } from '../../models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getNotifications(userId: number): Observable<Notifications[]> {
    return this.http.get<Notifications[]>(this.apiUrl + '/Dashboard/GetNotifications?userId=' + userId);
  }

  getPosts(userId: number): Observable<Posts[]> {
    return this.http.get<Posts[]>(this.apiUrl + '/Dashboard/GetPosts?userId=' + userId);
  }

  getFriends(userId: number): Observable<Friends[]> {
    return this.http.get<Friends[]>(this.apiUrl + '/Dashboard/GetFriends?userId=' + userId);
  }

  getFriendRequests(userId: number, resetNotification: boolean): Observable<Friends[]> {
    return this.http.get<Friends[]>(this.apiUrl + '/Dashboard/GetFriendRequests?userId=' + userId + '&resetNotification=' + resetNotification);
  }

  listAllPosts(postLimit: number): Observable<Posts[]> {
    return this.http.get<Posts[]>(this.apiUrl + '/Dashboard/ListAllPosts?postLimit=' + postLimit);
  }

  sendFriendRequest(userId: number, friendId: number): Observable<Friends> {
    return this.http.post<Friends>(this.apiUrl + '/Dashboard/SendFrientRequest?friendId=' + friendId, userId);
  }

  acceptFriendRequest(userId: number, friendId: number): Observable<Friends> {
    return this.http.post<Friends>(this.apiUrl + '/Dashboard/AcceptFriendRequest?friendId=' + friendId, userId);
  }

  submitPost(post: Posts, userId: number): Observable<Posts> {
    const data = post;
    return this.http.post<Posts>(this.apiUrl + '/Dashboard/SubmitPost?userId=' + userId, data);
  }

  updateBio(appUser: AppUser): Observable<AppUser> {
    return this.http.post<AppUser>(this.apiUrl + '/Dashboard/UpdateBio?userId=' + appUser.id, appUser);
  }

  deletePost(postId: number): Observable<boolean> {
    return this.http.delete<boolean>(this.apiUrl + '/Dashboard/' + postId);
  }

  searchUser(userName: string): Observable<AppUser> {
    return this.http.get<AppUser>(this.apiUrl + '/Dashboard/UserSearch?userName=' + userName);
  }

  grabAllUsers(): Observable<AppUser[]> {
    return this.http.get<AppUser[]>(this.apiUrl + '/Dashboard/GrabUsers');
  }

  submitComment(comment: Comments, postId: number, userName: string): Observable<Comments> {
    const data = comment;
    return this.http.post<Comments>(this.apiUrl + '/Dashboard/SubmitComment?postId=' + postId + '&userName=' + userName, data);
  }

  likePost(postId: number): Observable<number> {
    return this.http.post<number>(this.apiUrl + '/Dashboard/LikePost?postId=' + postId, postId);
  }

  rePost(postId: number, userId: number, rePostUser: string): Observable<Posts> {
    return this.http.post<Posts>(this.apiUrl + '/Dashboard/RePost?postId=' + postId + '&userId=' + userId + '&rePostUser=' + rePostUser, postId);
  }
}
