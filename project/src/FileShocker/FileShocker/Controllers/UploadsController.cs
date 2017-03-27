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
        private readonly IFileStorageService _fileStorageService;

        public UploadsController(IHostingEnvironment environment, ILoginManager loginManager, IFileStorageService fileStorageService)
        {
            _environment = environment;
            _loginManager = loginManager;
            _fileStorageService = fileStorageService;
        }

        
        // POST api/values
        [HttpPost("")]
        public async Task<IActionResult> Post(string username, string password, IFormFile file)
        {
            if (!ModelState.IsValid || !_loginManager.Login(username, password))
                return Unauthorized();

            var filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            await _fileStorageService.StoreFile(file, filename);
            
            return Created($"{filename}", filename);
        }
    }
}
