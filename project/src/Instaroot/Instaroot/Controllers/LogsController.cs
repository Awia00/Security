using System;
using System.Linq;
using System.Threading.Tasks;
using Instaroot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instaroot.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LogsController : Controller
    {
        private readonly ILoggingService _loggingService;

        public LogsController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public async Task<IActionResult> Index()
        {
            await _loggingService.LogInfo($"{User.Identity.Name} is viewing the log.");
            var logs = (await _loggingService.GetLog()).ToList();

            return View(logs);
        }
    }
}