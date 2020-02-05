using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }

        public int PostId { get; set; }
        [JsonIgnore]
        public Posts Post { get; set; }
    }
}
