import { Component, OnInit, HostListener } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import { Posts, Comments, AppUser } from 'src/models';
import { UserService } from '../services/user.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit {

  currentUser: AppUser = {};
  feed: Posts[];
  newComment: Comments = {};
  loggedUser: string;
  postLimit: number;

  constructor(private dashboardService: DashboardService, private userService: UserService, private toastr: ToastrService ) { }

  ngOnInit() {
    this.postLimit = 10;
    this.getUser();
    this.getFeed();
  }

  // Load more posts when bottom of page
  @HostListener("window:scroll", [])
  onScroll(): void {
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
      this.showMorePosts();
    }
  }


  getUser() {
    const me = this;
    this.userService.currentUser.subscribe(result => {
      me.currentUser = result;
      me.loggedUser = me.currentUser.userName;
    });
  }

  getFeed() {
    const me = this;
    this.dashboardService.listAllPosts(this.postLimit).subscribe(data => {
      me.feed = data;
    },
      err => {
        console.log(err);
      },
    );
  }

  onSubmitComment(postId: number) {
    const me = this;
    if (this.newComment.content == "")
      return;

    this.dashboardService.submitComment(this.newComment, postId, this.loggedUser).subscribe(data => {
      me.newComment.content = "";
      me.getFeed();
    },
      err => {
        console.log(err);
      },
    );
  }

  deletePost(postId: number) {
    const me = this;
    if (postId !== null) {
      this.dashboardService.deletePost(postId).subscribe(data => {
        me.toastr.success("Post Deleted");
        me.getFeed();
      },
        err => {
          console.log(err);
        },
      );
    }
  }

  likePost(postId: number) {
    const me = this;
    this.dashboardService.likePost(postId).subscribe(data => {
      me.getFeed();
    },
      err => {
        console.log(err);
      },
    );
  }

  rePost(postId: number, userId: number, rePostUser: string) {
    const me = this;
    this.dashboardService.rePost(postId, userId, rePostUser).subscribe(data => {
      me.getFeed();
      me.toastr.success("Successfully Re-Posted!");
    },
      err => {
        console.log(err);
      },
    );
  }

  showMorePosts() {
    this.postLimit += this.postLimit;
    this.getFeed();
  }
}
