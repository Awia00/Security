using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instaroot.Controllers
{
    [Authorize]
    public class FeedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
