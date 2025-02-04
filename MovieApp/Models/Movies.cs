﻿using Newtonsoft.Json;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieId { get; set; }
        public string Source { get; set; }
        public string MovieName { get; set; }
        public string MovieDetail { get; set; }
        public string MoviePicture { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
