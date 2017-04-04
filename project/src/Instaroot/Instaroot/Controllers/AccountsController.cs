using System.Linq;
using System.Threading.Tasks;
using Instaroot.Models;
using Instaroot.Services;
using Instaroot.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Instaroot.Controllers
{
    [Route("Accounts")]
    public class AccountsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILoggingService _loggingService;
        private readonly IImageService _imageService;

        public AccountsController(UserManager<User> userManager, SignInManager<User> signInManager, ILoggingService loggingService, IImageService imageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _loggingService = loggingService;
            _imageService = imageService;
        }

        [HttpGet("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                await _loggingService.LogTrace($"{model.UserName} is trying to log in.");
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    await _loggingService.LogTrace($"{model.UserName} logged in succesfully.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    await _loggingService.LogWarning($"{model.UserName} is locked out.");
                    return View("Lockout");
                }
                else
                {
                    await _loggingService.LogWarning($"{model.UserName} tried to login with wrong password {model.Password}.");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            await _loggingService.LogError("Invalid login attempt.");
            return View(model);
        }

        [HttpGet("SignUp")]
        public IActionResult SignUp(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost("SignUp")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;

                var user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName
                };

                await _loggingService.LogTrace($"{model.UserName} tries to sign up.");

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _loggingService.LogInfo($"{model.UserName} signed up succesfully.");

                    var root = _userManager.Users.SingleOrDefault(u => u.UserName == "instaroot");
                    if (root != null)
                    {
                        for (var i = -3; i < 0; i++)
                        {
                            try
                            {
                                await _imageService.Share(root, i, user.Id);
                            }
                            catch
                            {
                                // Ignore
                            }
                        }
                    }

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToLocal(returnUrl);
                }

                await _loggingService.LogError($"Problem aries while signing up {model.UserName}.");
                AddErrors(result);
            }

            await _loggingService.LogError("Invalid signup attempt.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            await _loggingService.LogTrace($"{_userManager.GetUserName(User)} left the building.");
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
