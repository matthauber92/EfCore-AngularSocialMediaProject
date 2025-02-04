﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Friends
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FriendId { get; set; }
        public bool IsFriend { get; set; }
        public int FriendSentId { get; set; }

        public int UserId { get; set; }
        [NotMapped]
        public ApplicationUser User { get; set; }
    }
}
