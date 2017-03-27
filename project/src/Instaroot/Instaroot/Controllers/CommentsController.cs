using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Instaroot.Models;
using Instaroot.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Instaroot.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public CommentsController(ICommentService commentService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int commentId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                await _commentService.DeleteComment(new Comment
                {
                    User = user,
                    Id = commentId
                });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Post(int imageId, string comment)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                
                await _commentService.PostComment(new Comment
                {
                    Text = comment,
                    ImageId = imageId,
                    User = user,
                    TimeStamp = DateTime.Now,
                });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
