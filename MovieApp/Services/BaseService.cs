using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MovieApp.Models;

namespace MovieApp.Services
{
    public class BaseService
    {
        public APIContext _db;
        public IHttpContextAccessor _context;

        protected BaseService(APIContext db)//, IHttpContextAccessor context)
        {
            _db = db;
            //_context = context;
        }
    }
}
