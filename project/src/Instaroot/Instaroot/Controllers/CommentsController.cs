using System;
using System.Threading.Tasks;
using Instaroot.Models;
using Instaroot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Instaroot.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        private readonly ILoggingService _loggingService;

        public CommentsController(ICommentService commentService, UserManager<User> userManager, ILoggingService loggingService)
        {
            _commentService = commentService;
            _userManager = userManager;
            _loggingService = loggingService;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int commentId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                await _commentService.DeleteComment(user, commentId);
                await _loggingService.LogInfo($"{user.UserName} deleted comment with id {commentId}");
            }
            else
            {
                await _loggingService.LogError($"{_userManager.GetUserName(User)} had issues deleting a comment.");
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
                await _loggingService.LogInfo($"{user.UserName} succesfully posted a comment to image with id {imageId}.");
            }
            else
            {
                await _loggingService.LogError($"{_userManager.GetUserName(User)} had issues posting a comment.");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
