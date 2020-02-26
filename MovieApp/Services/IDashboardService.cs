﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Models;
using MovieApp.Helpers;

namespace MovieApp.Services
{
    public interface IDashboardService
    {
        Result<ApplicationUser> Search(string userName);
        Result<List<Posts>> ListUserPosts(int userId);
        Result<List<Posts>> ListAllUserPosts();
        Result<ApplicationUser> UpdateBio(ApplicationUser userBio);
        Result<Posts> SubmitUserPost(Posts post, int userId);
        Result<Posts> RePost(int postId, int userId, string rePostUser);
        Result<bool> DeletePost(int postId);
        Result<Comments> SubmitUserComment(Comments comment, int postId, string userName);
        Result<int> LikePost(int postId);
    }
}
