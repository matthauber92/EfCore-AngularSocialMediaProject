using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Notifications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }
        public int Messages { get; set; }
        public int FriendRequests { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
