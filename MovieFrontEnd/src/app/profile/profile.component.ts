import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import { Posts, Comments } from 'src/models';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../services/user.service';
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  @Input() currentUser;
  userPosts: Posts[];
  newPost: Posts = {};
  newComment: Comments = {};
  @ViewChild('collapseBio', { static: false }) bioCollapse: ElementRef;
  @Input() loggedUser;
  @Input() userName;

  constructor(private dashboardService: DashboardService, private userService: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.getPosts();
    console.log(this.currentUser);
  }

  ngOnChanges() {
    this.getPosts();
  }

  //getUser() {
  //  const me = this;
  //  this.userService.currentUser.subscribe(result => {
  //    me.currentUser = result;
  //    //me.loggedUser = me.currentUser.id;
  //    //me.userName = me.currentUser.userName;
  //  },
  //    err => {
  //      console.log(err);
  //    },
  //  );
  //}

  getPosts() {
    const me = this;
    this.dashboardService.getPosts(this.currentUser.id).subscribe(data => {
      me.userPosts = data;
    },
      err => {
        console.log(err);
      },
    );
  }

  updateBio() {
    const me = this;
    //console.log(this.currentUser.bio);
    this.dashboardService.updateBio(this.currentUser).subscribe(data => {
      console.log(data);
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

    this.dashboardService.submitComment(this.newComment, postId, this.userName).subscribe(data => {
      me.newComment.content = "";
      me.getPosts();
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

}
