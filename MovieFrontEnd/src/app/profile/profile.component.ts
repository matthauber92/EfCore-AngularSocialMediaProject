import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';
import { DashboardService } from '../services/dashboard.service';
import { Posts } from 'src/models';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  @Input() currentUser;
  userPosts: Posts[];
  newPost: Posts = {};
  @ViewChild('collapseBio', { static: false }) bioCollapse: ElementRef;

  constructor(private dashboardService: DashboardService, private toastr: ToastrService) { }

  ngOnInit() {
    this.getPosts();
  }

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

}
