using System.Linq;
using System.Threading.Tasks;
using Instaroot.Models;
using Instaroot.Services;
using Instaroot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Instaroot.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public HomeController(IImageService imageService, UserManager<User> userManager, IUserService userService)
        {
            _imageService = imageService;
            _userManager = userManager;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var images = await _imageService.GetImages(userId);
            ViewBag.Usernames = (await _userService.GetUsers()).Select(user => user.UserName);
            return View(images.OrderByDescending(image => image.TimeStamp).Select(image => new ImageViewModel
            {
                IsOwner = image.Owner.Id == userId,
                Comments = image.Comments.OrderByDescending(comment => comment.TimeStamp).Select(comment => new CommentViewModel
                {
                    Text = comment.Text,
                    IsAuthor = comment.User.Id == userId,
                    Id = comment.Id
                }).ToList(),
                ImageUrl = image.Path,
                SharedWithUsers = image.Users.Select(user => user.User.UserName).ToList()
            }).ToList());
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
