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
    }
}