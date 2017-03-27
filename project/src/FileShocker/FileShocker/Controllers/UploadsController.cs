using System;
using System.IO;
using System.Threading.Tasks;
using FileShocker.Services;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileShocker.Controllers
{
    [Route("uploads")]
    public class UploadsController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly ILoginManager _loginManager;

        public UploadsController(IHostingEnvironment environment, ILoginManager loginManager)
        {
            _environment = environment;
            _loginManager = loginManager;
        }

        
        // POST api/values
        [HttpPost("")]
        public async Task<IActionResult> Post(string username, string password, IFormFile file)
        {
            if (!_loginManager.Login(username, password)) return Unauthorized();

            var filename = $"{Guid.NewGuid()}.{Path.GetExtension(file.FileName)}";

            using (
                var filestream = new FileStream(Path.Combine(_environment.WebRootPath, "uploads", filename), FileMode.CreateNew))
            {
                await file.CopyToAsync(filestream);
            }

            return Created($"http://{Request.GetUri().Host}/uploads/{filename}", filename);
        }
    }
}
