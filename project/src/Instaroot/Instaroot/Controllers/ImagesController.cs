using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public ImagesController(IFileShockerService fileShockerService, IImageService imageService, UserManager<User> userManager)
        {
            _fileShockerService = fileShockerService;
            _imageService = imageService;
            _userManager = userManager;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int imageId)
        {
            if (ModelState.IsValid)
            {
                await _imageService.DeleteImage(new Image
                {
                    Owner = await _userManager.GetUserAsync(User),
                    TimeStamp = DateTime.Now
                });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile image)
        {
            if (ModelState.IsValid && image.ContentType.StartsWith("image/"))
            {
                var imageUrl = await _fileShockerService.UploadImage(image);

                var uri = Request.GetUri();
                var port = uri.Port == 80 ? "" : $":{uri.Port}";
                imageUrl = $"http://{uri.Host}{port}/uploads/{imageUrl}";

                await _imageService.PostImage(new Image
                {
                    Owner = await _userManager.GetUserAsync(User),
                    Path = imageUrl,
                    TimeStamp = DateTime.Now
                });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
