using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Friends
    {
        [Key]
        public int FriendId { get; set; }
        public int FriendLink { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
