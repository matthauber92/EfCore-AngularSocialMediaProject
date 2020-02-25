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

        //Return List of Recent Posts for Feed
        public Result<List<Posts>> ListAllUserPosts()
        {
            Result<List<Posts>> result = new Result<List<Posts>>();
            try
            {
                result.Value = _db.Posts.Include(u => u.User).Include(c => c.Comments).OrderByDescending(o => o.PostId).ToList();
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

        //Update User Bio
        public Result<ApplicationUser> UpdateBio(ApplicationUser userBio)
        {
            Result<ApplicationUser> result = new Result<ApplicationUser>();
            try
            {
             
                using (var transaction = _db.Database.BeginTransaction())
                {
                    var user = GetUserById(userBio.Id);
                    //Needs Reviewed - has Bio?
                    if (!user.HasValue)
                        user.Value.Bio = userBio.Bio;
                    else
                        user.Value.Bio = userBio.Bio;
                    _db.SaveChanges();

                    transaction.Commit();
                }
                result.Value = userBio;
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
                result.Value = _db.Users.Where(u => u.UserName == userName || u.DisplayName == userName || u.UserName.StartsWith(userName) || u.UserName.EndsWith(userName)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        // Get User
        public Result<ApplicationUser> GetUserById(int userId)
        {
            Result<ApplicationUser> result = new Result<ApplicationUser>();
            try
            {
                result.Value = _db.Users.Where(u => u.Id == userId).FirstOrDefault();
                if (result.Value == null)
                {
                    throw new Exception("No user for id: " + userId);
                }
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        //Submit User Comment
        public Result<Comments> SubmitUserComment(Comments comment, int postId, string userName)
        {
            Result<Comments> result = new Result<Comments>();
            try
            {
                Comments model = new Comments();
                using (var transaction = _db.Database.BeginTransaction())
                {
                    comment.PostId = postId;
                    comment.UserName = userName;
                    _db.Comments.Add(comment);
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

        //Like User Post
        public Result<int> LikePost(int postId)
        {
            Result<int> result = new Result<int>();
            try
            {
                int model = new int();
                using (var transaction = _db.Database.BeginTransaction())
                {
                    var post = _db.Posts.Where(p => p.PostId == postId).FirstOrDefault();
                    post.Likes++;
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
    }
}
