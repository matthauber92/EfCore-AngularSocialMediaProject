using System.Collections.Generic;
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
        [Route("GetNotifications")]
        public ActionResult<Notifications> SendFriendRequest([FromQuery] int userId)
        {
            var result = _service.GetNotifications(userId);

            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                return null;
            }
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
                return null;
            }
        }

        [HttpPost]
        [Route("SendFrientRequest")]
        public ActionResult<Friends> SendFriendRequest([FromBody] int userId, int friendId)
        {
            var result = _service.SendFriendRequest(userId, friendId);

            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Route("GetFriendRequests")]
        public ActionResult<List<Friends>> GetFriendRequests(int userId)
        {
            var result = _service.GetFriendRequests(userId);

            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Route("ListAllPosts")]
        public ActionResult<List<Posts>> ListAllPosts(int postLimit)
        {
            var result = _service.ListAllUserPosts(postLimit);

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
        [Route("RePost")]
        public ActionResult<Posts> RePost([FromBody] int postId, int userId, string rePostUser)
        {
            var result = _service.RePost(postId, userId, rePostUser);

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
        public ActionResult<ApplicationUser> UpdateBio([FromBody] ApplicationUser userBio)
        {
            var result = _service.UpdateBio(userBio);

            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                return ErrorResult(result.Exception.Message);
            }
        }

        [HttpGet]
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

        [HttpGet]
        [Route("GrabUsers")]
        public ActionResult<List<ApplicationUser>> GrabUsers()
        {
            var result = _service.GrabUsers();

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
        [Route("SubmitComment")]
        public ActionResult<Comments> SubmitComment([FromBody] Comments comment, int postId, string userName)
        {
            var result = _service.SubmitUserComment(comment, postId, userName);

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
        [Route("LikePost")]
        public ActionResult<int> LikePost(int postId)
        {
            var result = _service.LikePost(postId);

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