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
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Bio { get; set; }
        public string Picture { get; set; }


        [NotMapped]
        public List<Notifications> Notifications { get; set; }

        [NotMapped]
        public List<Posts> Posts { get; set; }

        [NotMapped]
        public List<Movies> Movies { get; set; }

        [NotMapped]
        public List<Friends> Friends { get; set; }
    }

}
