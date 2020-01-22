using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MovieApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Bio { get; set; }
        public string Picture { get; set; }

        public List<Posts> Posts { get; set; }

        public List<Movies> Movies { get; set; }

        public List<Friends> Friends { get; set; }
    }

}
