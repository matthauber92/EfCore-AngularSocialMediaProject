using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Models;
using MovieApp.Helpers;

namespace MovieApp.Services
{
    public interface IDashboardService
    {
        Result<List<Posts>> ListUserPosts(int userId);
    }
}
