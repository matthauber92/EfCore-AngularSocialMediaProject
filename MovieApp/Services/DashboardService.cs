using MovieApp.Models;
using MovieApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MovieApp.Services
{
    public class DashboardService : BaseService, IDashboardService
    {
        public DashboardService(APIContext db) : base(db)
        {
        }

        //Return List of User Posts
        public Result<List<Posts>> ListUserPosts(int userId)
        {
            Result<List<Posts>> result = new Result<List<Posts>>();
            try
            {
                result.Value = _db.Posts.Where(x => x.UserId == userId).Include(co => co.Comments).OrderByDescending(o => o.PostId).ToList();
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        // Delete user posts
        public Result<bool> DeletePost(int postId)
        {
            Result<bool> result = new Result<bool>();
            try
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    var post = _db.Posts.FirstOrDefault(p => p.PostId == postId);
                    _db.Remove(post);
                    _db.SaveChanges();

                    transaction.Commit();
                }
                result.Value = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        //Submit User Post
        public Result<Posts> SubmitUserPost(Posts post, int userId)
        {
            Result<Posts> result = new Result<Posts>();
            try
            {
                Posts model = new Posts();
                using (var transaction = _db.Database.BeginTransaction())
                {
                    post.UserId = userId;
                    _db.Posts.Add(post);
                    _db.SaveChanges();

                    transaction.Commit();
                }
                    result.Value = model;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        //Submit User Post
        public Result<string> UpdateBio(int userId, string bio)
        {
            Result<string> result = new Result<string>();
            try
            {
             
                using (var transaction = _db.Database.BeginTransaction())
                {
                    var user = _db.Users.Where(u => u.Id == userId).FirstOrDefault();
                    if (user.Bio == "")
                        user.Bio += bio;
                    else
                        user.Bio = bio;
                    _db.SaveChanges();

                    transaction.Commit();
                }
                result.Value = bio;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        //Search Users
        public Result<ApplicationUser> Search(string userName)
        {
            Result<ApplicationUser> result = new Result<ApplicationUser>();
            try
            {
                result.Value = _db.Users.Where(u => u.UserName == userName || u.DisplayName == userName || u.UserName.StartsWith(userName) && u.UserName.EndsWith(userName)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
    }
}
