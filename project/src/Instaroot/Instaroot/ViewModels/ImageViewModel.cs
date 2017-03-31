using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Instaroot.ViewModels
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        public bool IsOwner { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public List<UserViewModel> SharedWithUsers { get; set; }
    }
}
