using System.Linq;
using System.Threading.Tasks;
using Instaroot.Models;
using Instaroot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Instaroot.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IImageService _imageService;
        private readonly UserManager<User> _userManager;

        public HomeController(IImageService imageService, UserManager<User> userManager)
        {
            _imageService = imageService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var images = await _imageService.GetImages(userId);
            return View(images.ToList());
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
