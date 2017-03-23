using Common.Models;
using Instaroot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Instaroot.Controllers
{
    [Authorize]
    public class FeedController : Controller
    {
        private readonly IImageService _imageService;
        private readonly UserManager<User> _userManager;

        public FeedController(IImageService imageService, UserManager<User> userManager)
        {
            _imageService = imageService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            try
            {
                var images = await _imageService.GetImages(userId);
                return View(images.ToList());
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}
