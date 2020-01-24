using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieApp.Controllers
{
    public abstract class BaseController<T, S> : Controller where T : class
    {
        protected readonly S _service;
        public BaseController(S service)
        {
            _service = service;
        }

        protected JsonResult ErrorResult(string errorMessage)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Json(new { error = errorMessage });
        }

    }
}