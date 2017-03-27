using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Instaroot.Services
{
    public class FileShockerService : IFileShockerService
    {
        private const string FileshockerUrl = "http://fileshocker.com:100/upload";
        public async Task<string> UploadImage(IFormFile image)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(new Uri(FileshockerUrl),
                    new StreamContent(image.OpenReadStream()));
                if (response.IsSuccessStatusCode) {
                    return response.Headers.Location.ToString();
                }
                throw new Exception("Fileshocker was shocked todo"); // todo
            }
        }
    }
}