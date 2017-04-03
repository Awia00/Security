using Microsoft.AspNetCore.Mvc;

namespace FileShocker.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}