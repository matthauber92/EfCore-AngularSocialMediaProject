using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Movies
    {
        [Key]
        public int MovieId { get; set; }
        public string Source { get; set; }
        public string MovieName { get; set; }
        public string MovieDetail { get; set; }
        public string MoviePicture { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
