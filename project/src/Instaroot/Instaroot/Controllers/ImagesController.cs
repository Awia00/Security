using System;
using System.Threading.Tasks;
using Instaroot.Models;
using Instaroot.Services;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Instaroot.Controllers
{
    [Authorize]
    public class ImagesController : Controller
    {
        private readonly IFileShockerService _fileShockerService;
        private readonly IImageService _imageService;
        private readonly UserManager<User> _userManager;
        private readonly ILoggingService _loggingService;

        public ImagesController(IFileShockerService fileShockerService, IImageService imageService, UserManager<User> userManager, ILoggingService loggingService)
        {
            _fileShockerService = fileShockerService;
            _imageService = imageService;
            _userManager = userManager;
            _loggingService = loggingService;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int imageId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                await _imageService.DeleteImage(user, imageId);
                await _loggingService.LogInfo($"{user.UserName} deleted image with id {imageId}.");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Share(string shareWithId, int imageId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                await _imageService.Share(user, imageId, shareWithId);
                await _loggingService.LogInfo($"{user.UserName} now shares image with id {imageId} with user with id {shareWithId}.");
            }
            return this.RedirectToRegion("Index", "Home", $"image{imageId}");
        }
        [HttpPost]
        public async Task<IActionResult> UnShare(string sharedWithId, int imageId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                await _imageService.Unshare(user, imageId, sharedWithId);
                await _loggingService.LogInfo($"{user.UserName} removed sharing of image with id {imageId} from user with id {sharedWithId}.");
            }
            return this.RedirectToRegion("Index", "Home", $"image{imageId}");
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile image)
        {
            var imageId = 0;
            if (ModelState.IsValid && image.ContentType.StartsWith("image/"))
            {
                await _loggingService.LogTrace("Received well-formatted image.");
                var imageUrl = await _fileShockerService.UploadImage(image);

                var uri = Request.GetUri();
                var port = uri.Port == 80 ? "" : $":{uri.Port}";
                imageUrl = $"http://{uri.Host}{port}/uploads/{imageUrl}";

                await _loggingService.LogTrace($"About to upload image to FileShocker on url: {imageUrl}.");
                imageId = await _imageService.PostImage(new Image
                {
                    Owner = await _userManager.GetUserAsync(User),
                    Path = imageUrl,
                    TimeStamp = DateTime.Now
                });
                await _loggingService.LogInfo($"Image with id {imageId} uploaded at FileShocker succesfully.");
            }
            return this.RedirectToRegion("Index", "Home", $"image{imageId}");
        }
    }
}
