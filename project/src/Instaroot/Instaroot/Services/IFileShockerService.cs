using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Instaroot.Services
{
    public interface IFileShockerService
    {
        Task<string> UploadImage(IFormFile image);
    }
}
