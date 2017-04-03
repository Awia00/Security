using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Instaroot.Controllers
{
    public static class ControllerExtensions
    {
        public static IActionResult RedirectToRegion(this Controller ctrl, string action, string controller, string region)
        {
            return new RedirectResult(ctrl.Url.Action(action, controller) + $"#{region}");
        }
    }
}
