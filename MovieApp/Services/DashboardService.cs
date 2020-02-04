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

        //Submit User Post
        public Result<Posts> SubmitUserPost(Posts post)
        {
            Result<Posts> result = new Result<Posts>();
            try
            {
                Posts model = new Posts();
                using (var transaction = _db.Database.BeginTransaction())
                {
                    if (post.PostId == 0)
                    {
                        model.PostId = post.PostId;
                        model.Content = post.Content;
                        model.UserId = post.UserId;
                        model.Likes = 0;
                        model.Comments = new List<Comments>();
                        model.User = post.User;

                        _db.Posts.Add(model);
                        _db.SaveChanges();
                    }
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
