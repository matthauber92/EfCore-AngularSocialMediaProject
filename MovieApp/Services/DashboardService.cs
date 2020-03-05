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

        //Get User Notifications
        public Result<Notifications> GetNotifications(int userId)
        {
            Result<Notifications> result = new Result<Notifications>();
            try
            {
                result.Value = _db.Notifications.Where(n => n.UserId == userId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        //Return List of Recent Posts for Feed
        public Result<List<Posts>> ListAllUserPosts(int postLimit)
        {
            Result<List<Posts>> result = new Result<List<Posts>>();
            try
            {
                result.Value = _db.Posts.Include(u => u.User).Include(c => c.Comments).OrderByDescending(o => o.PostId).Take(postLimit).ToList();
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        //Get Friends
        public Result<List<Friends>> GetFriends(int userId)
        {
            Result<List<Friends>> result = new Result<List<Friends>>();
            try
            {
                result.Value = _db.Friends.Where(u => u.FriendSentId == userId && u.IsFriend == true).Include(u => u.User).ToList();
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        //Send Friend Request
        public Result<Friends> SendFriendRequest(int userId, int friendId)
        {
            Result<Friends> result = new Result<Friends>();
            try
            {
                Friends model = new Friends();
                Notifications newNotification = new Notifications();
                using (var transaction = _db.Database.BeginTransaction())
                {
                    model.UserId = userId;
                    model.FriendSentId = friendId;
                    model.IsFriend = false;

                    // update notifications of users currentUser sent rquest to
                    var notifications = _db.Notifications.Where(u => u.UserId == friendId).FirstOrDefault();

                    if(notifications == null)
                    {
                        newNotification.UserId = friendId;
                        newNotification.FriendRequests++;
                        _db.Notifications.Add(newNotification);
                    }
                    else
                    {
                        notifications.FriendRequests += 1;
                    }

                    _db.Friends.Add(model);

                    _db.SaveChanges();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        //Send Friend Request
        public Result<List<Friends>> GetFriendRequests(int userId, bool resetNotification)
        {
            Result<List<Friends>> result = new Result<List<Friends>>();
            try
            {
                result.Value = _db.Friends.Where(u => u.FriendSentId == userId && u.IsFriend == false).Include(u => u.User).ToList();

                if (result.HasValue && resetNotification)
                {
                    using (var transaction = _db.Database.BeginTransaction())
                    {
                        var resetNotifications = _db.Notifications.Where(u => u.UserId == userId).FirstOrDefault();
                        resetNotifications.FriendRequests = 0;

                        _db.SaveChanges();

                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        //Accept Friend Request
        public Result<Friends> AcceptFriendRequests(int userId, int friendId)
        {
            Result<Friends> result = new Result<Friends>();
            try
            {
                var newFriend = _db.Friends.Where(u => u.FriendSentId == userId && u.IsFriend == false && u.UserId == friendId).Include(u => u.User).FirstOrDefault();

                if (newFriend != null)
                {
                    using (var transaction = _db.Database.BeginTransaction())
                    {
                        newFriend.IsFriend = true;

                        _db.SaveChanges();

                        transaction.Commit();
                    }
                }
                result.Value = newFriend;
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

        //RePost User Post
        public Result<Posts> RePost(int postId, int userId, string rePostUser)
        {
            Result<Posts> result = new Result<Posts>();
            try
            {
                Posts model = new Posts();
                using (var transaction = _db.Database.BeginTransaction())
                {
                    var post = _db.Posts.Where(p => p.PostId == postId).FirstOrDefault();
                    model.UserId = userId;
                    model.RePostUser = rePostUser;
                    model.Content = post.Content;
                    _db.Posts.Add(model);
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

        //Grab All Users
        public Result<List<ApplicationUser>> GrabUsers()
        {
            Result<List<ApplicationUser>> result = new Result<List<ApplicationUser>>();
            try
            {
                result.Value = _db.Users.OrderBy(o => o.Id).ToList();
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
