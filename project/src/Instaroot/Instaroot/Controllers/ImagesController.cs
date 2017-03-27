using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Instaroot.Models;
using Instaroot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Instaroot.Controllers
{
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
            if (ModelState.IsValid)
            {
                switch (Path.GetExtension(image.FileName).ToLower())
                {
                    case "png":
                    case "jpg":
                    case "jpeg":
                    case "bmp":
                    case "gif":
                        var imageUrl = await _fileShockerService.UploadImage(image);
                        await _imageService.PostImage(new Image
                        {
                            Owner = await _userManager.GetUserAsync(User),
                            Path = imageUrl,
                            TimeStamp = DateTime.Now
                        });
                        break;
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
