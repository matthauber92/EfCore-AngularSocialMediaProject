﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Posts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public string RePostUser { get; set; }

        public int UserId { get; set; }
        [NotMapped]
        public ApplicationUser User { get; set; }

        public List<Comments> Comments { get; set; }
    }
}
