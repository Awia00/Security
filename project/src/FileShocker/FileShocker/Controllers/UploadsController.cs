using System;
using System.IO;
using System.Threading.Tasks;
using FileShocker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileShocker.Controllers
{
    [Route("uploads")]
    public class UploadsController : Controller
    {
        private readonly IFileStorageService _fileStorageService;

        public UploadsController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        
        // POST api/values
        [HttpPost("")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (!ModelState.IsValid) return BadRequest();

            var filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            await _fileStorageService.StoreFile(file, filename);
            
            return Created($"{filename}", filename);
        }
    }
}
