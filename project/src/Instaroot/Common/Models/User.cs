using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Common.Models
{
    public class User : IdentityUser
    {
        // Relations
        public ICollection<Image> OwnedImages { get; set; }
        public ICollection<ImageUser> AccessibleImages { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
