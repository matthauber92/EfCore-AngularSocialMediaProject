import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import { Posts, Comments, AppUser } from 'src/models';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../services/user.service';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  currentUser: AppUser = {};
  userPosts: Posts[];
  newPost: Posts = {};
  newComment: Comments = {};
  @ViewChild('collapseBio', { static: false }) bioCollapse: ElementRef;
  @ViewChild('collapseComments', { static: false }) commentCollapse: ElementRef;
  loggedUser: string;
  userSearched: string;
  userProfile: AppUser = {};

  constructor(private dashboardService: DashboardService, private route: ActivatedRoute, private userService: UserService, private toastr: ToastrService) {
    const me = this;
    this.route.params.subscribe(params => {
      me.userSearched = params.id;
      me.searchUser();
    });
  }

  ngOnInit() {
    if (this.loggedUser == this.currentUser.userName || this.userSearched == this.currentUser.userName) {
      this.getUser();
    } else {
      this.getPosts();
    }
  }

  getUser() {
    const me = this;
    this.userService.currentUser.subscribe(result => {
      me.currentUser = result;
      me.loggedUser = me.currentUser.userName;
      me.getPosts();
    });
  }

  searchUser() {
    const me = this;
    if (this.userSearched !== this.currentUser.userName) {
      this.dashboardService.searchUser(this.userSearched).subscribe(result => {
        me.userProfile = result;
        me.getPosts();
      },
        err => {
          console.log(err);
        },
      );
    } else {
      this.getPosts();
    }
  }

  getPosts() {
    const me = this;
    if (this.userSearched !== this.loggedUser) {
      this.dashboardService.getPosts(this.userProfile.id).subscribe(data => {
        me.userPosts = data;
      },
        err => {
          console.log(err);
        },
      );
    } else {
      this.dashboardService.getPosts(this.currentUser.id).subscribe(data => {
        me.userPosts = data;
      },
        err => {
          console.log(err);
        },
      );
    }
  }

  updateBio() {
    const me = this;
    this.dashboardService.updateBio(this.currentUser).subscribe(data => {
      me.toastr.success("Bio Successfully Updated");
      me.bioCollapse.nativeElement.click();
    },
      err => {
        console.log(err);
        me.toastr.error("Bio Could Not Be Updated");
      },
    );
  }

  deletePost(postId: number) {
    const me = this;
    if (postId !== null) {
      this.dashboardService.deletePost(postId).subscribe(data => {
        me.toastr.success("Post Deleted");
        me.getPosts();
      },
        err => {
          console.log(err);
        },
      );
    }
  }

  onSubmit() {
    const me = this;
    if (this.newPost.content == "")
      return;

    this.dashboardService.submitPost(this.newPost, this.currentUser.id).subscribe(data => {
      me.toastr.success("Added New Post!");
      me.newPost.content = "";
      me.getPosts();
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
      me.getPosts();

      let commentPost = me.userPosts.find(p => p.postId == postId);
      //commentPost.showMe = true;
      console.log(commentPost)
    },
      err => {
        console.log(err);
      },
    );
  }

  likePost(postId: number) {
    const me = this;
    this.dashboardService.likePost(postId).subscribe(data => {
      me.getPosts();
    },
      err => {
        console.log(err);
      },
    );
  }

  rePost(postId: number, userId: number, rePostUser: string) {
    const me = this;
    this.dashboardService.rePost(postId, userId, rePostUser).subscribe(data => {
      me.getPosts();
      me.toastr.success("Successfully Re-Posted!");
    },
      err => {
        console.log(err);
      },
    );
  }

}
