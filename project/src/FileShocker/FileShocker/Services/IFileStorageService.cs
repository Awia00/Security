using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FileShocker.Services
{
    public interface IFileStorageService
    {
        Task StoreFile(IFormFile file, string fileName);
    }
}
