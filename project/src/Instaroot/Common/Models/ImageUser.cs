using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ImageUser
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
