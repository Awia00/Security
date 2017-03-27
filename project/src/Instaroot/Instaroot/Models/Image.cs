using System;
using System.Collections.Generic;

namespace Instaroot.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public DateTime TimeStamp { get; set; }

        // Relations
        public User Owner { get; set; }
        public ICollection<ImageUser> Users { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
