﻿using MovieApp.Models;
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
    }
}
