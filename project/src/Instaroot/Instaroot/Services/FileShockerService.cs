using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Instaroot.Services
{
    public class FileShockerService : IFileShockerService
    {
        private readonly string _fileshockerAddress;

        public FileShockerService(string fileshockerAddress)
        {
            _fileshockerAddress = fileshockerAddress;
        }
        
        public async Task<string> UploadImage(IFormFile image)
        {
            using (var client = new HttpClient())
            {
                var imageStream = image.OpenReadStream();
                using (var binaryReader = new BinaryReader(imageStream))
                {
                    var data = binaryReader.ReadBytes((int)imageStream.Length);
                    var multipartFormContent = new MultipartFormDataContent();
                    var bytes = new ByteArrayContent(data);
                    multipartFormContent.Add(bytes, "file", image.FileName);
                    var response = await client.PostAsync(new Uri(_fileshockerAddress), multipartFormContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Headers.Location.ToString();
                    }
                }
                throw new Exception("Fileshocker was shocked todo"); // todo
            }
        }
    }
}