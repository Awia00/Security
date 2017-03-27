using System;

namespace Instaroot.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }

        // Relations
        public User User { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
