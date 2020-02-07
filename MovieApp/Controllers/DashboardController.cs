﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MovieApp.Models;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController<DashboardController, IDashboardService>
    {
        private readonly ApplicationSettings _appSettings;

        public DashboardController(IDashboardService service, IOptions<ApplicationSettings> appSettings) : base(service)
        {
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        [Route("GetPosts")]
        public ActionResult<List<Posts>> GetPosts([FromQuery] int userId)
        {
            var result = _service.ListUserPosts(userId);

            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                return ErrorResult(result.Exception.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> DeletePost(int id)
        {
            var result = _service.DeletePost(id);
            if (result.HasValue)
                return result.Value;
            else
                return ErrorResult(result.Exception.Message);
        }

        [HttpPost]
        [Route("SubmitPost")]
        public ActionResult<Posts> SubmitPost([FromBody] Posts post, int userId)
        {
            var result = _service.SubmitUserPost(post, userId);

            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                return ErrorResult(result.Exception.Message);
            }
        }

        [HttpPost]
        [Route("UpdateBio")]
        public ActionResult<string> UpdateBio(int userId, [FromBody] string bio)
        {
            var result = _service.UpdateBio(userId, bio);

            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                return ErrorResult(result.Exception.Message);
            }
        }

        [HttpPost]
        [Route("UserSearch")]
        public ActionResult<ApplicationUser> Search([FromQuery] string userName)
        {
            var result = _service.Search(userName);

            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                return ErrorResult(result.Exception.Message);
            }
        }
    }
}