using System.Linq;
using System.Threading.Tasks;
using Instaroot.Models;
using Instaroot.Services;
using Instaroot.ViewModels;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
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
        private readonly ILoggingService _loggingService;

        public HomeController(IImageService imageService, UserManager<User> userManager, IUserService userService, ILoggingService loggingService)
        {
            _imageService = imageService;
            _userManager = userManager;
            _userService = userService;
            _loggingService = loggingService;
        }
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            await _loggingService.LogInfo($"{loggedInUser} is accessing the feed.");

            var images = await _imageService.GetImages(loggedInUser.Id);
            ViewBag.Username = new UserViewModel
            {
                Id = loggedInUser.Id,
                Username = loggedInUser.UserName
            };
            ViewBag.Usernames = (await _userService.GetUsers()).Select(user => new UserViewModel
            {
                Id = user.Id,
                Username = user.UserName
            });

            var uri = Request.GetUri();
            var portString = uri.Port != 80 ? $":{uri.Port}" : "";
            var uriBase = $"{uri.Scheme}://{uri.Host}{portString}/";

            return View(images.OrderByDescending(image => image.TimeStamp).Select(image => new ImageViewModel
            {
                Id = image.Id,
                IsOwner = image.Owner.Id == loggedInUser.Id,
                Username = image.Owner.UserName,
                Comments = image.Comments.OrderByDescending(comment => comment.TimeStamp).Select(comment => new CommentViewModel
                {
                    Text = comment.Text,
                    Author = comment.User.UserName,
                    IsAuthor = comment.User.Id == loggedInUser.Id,
                    Id = comment.Id,
                    ImageId = image.Id
                }).ToList(),
                ImageUrl = uriBase + image.Path,
                SharedWithUsers = image.Users.Select(user => new UserViewModel
                {
                    Id = user.User.Id,
                    Username = user.User.UserName
                }).ToList()
            }).ToList());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Error()
        {
            await _loggingService.LogError("Someone goofed :-(");

            return View();
        }
    }
}
