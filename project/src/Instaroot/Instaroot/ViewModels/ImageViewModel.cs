using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Instaroot.ViewModels
{
    public class ImageViewModel
    {
        public bool IsOwner { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
        public ICollection<string> SharedWithUsers { get; set; }
    }
}
