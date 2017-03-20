using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class User
    {
        public int Id { get; set; } 
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        // Relations
        public ICollection<Image> OwnedImages { get; set; }
        public ICollection<ImageUser> AccessibleImages { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
