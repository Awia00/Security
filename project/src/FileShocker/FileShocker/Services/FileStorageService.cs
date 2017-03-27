using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FileShocker.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _fileRootPath;

        public FileStorageService(string fileRootPath)
        {
            this._fileRootPath = fileRootPath;
        }

        public async Task StoreFile(IFormFile file, string fileName)
        {
            using (
                var filestream = new FileStream(Path.Combine(_fileRootPath, fileName), FileMode.CreateNew))
            {
                await file.CopyToAsync(filestream);
            }
        }
    }
}