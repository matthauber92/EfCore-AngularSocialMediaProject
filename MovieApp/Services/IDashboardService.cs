using System;
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
        Result<string> UpdateBio(int userId, string bio);
        Result<Posts> SubmitUserPost(Posts post, int userId);
        Result<bool> DeletePost(int postId);
    }
}
