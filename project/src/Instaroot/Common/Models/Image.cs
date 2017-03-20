using System.Collections.Generic;

namespace Common.Models
{
    public class Image
    {
        public string Path { get; set; }

        // Relations
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
