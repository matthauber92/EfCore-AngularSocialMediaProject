using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieApp.Models;
using MovieApp.Services;
using MovieApp.Helpers;

namespace MovieApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : BaseController<AppUserController, IAppUserService>
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private static IAppUserService service;
        private readonly ApplicationSettings _appSettings;

        public AppUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationSettings appSettings):base(service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings;
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<Object> PostNewUser(ApplicationUserModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(AppUser model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //User Authentification
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
                return new NotFoundResult();
        }

        [HttpGet]
        [Authorize]
        [Route("UserProfile")]
        public async Task<Object> GetUser()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new 
            {
                user.Id,
                user.UserName,
                user.Bio,
                user.DisplayName,
                user.FirstName,
                user.LastName,
                user.Email
            };
        }

        [HttpGet]
        [Route("Dash")]
        public ActionResult<List<Posts>> GetPosts([FromQuery] int userId)
        {
            var result = _service.ListUserPosts(userId);

            if(result.HasValue)
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