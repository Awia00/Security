using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Instaroot.Services
{
    public class FileShockerService : IFileShockerService
    {
        private const string FileshockerUrl = "http://localhost:51266/uploads";
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
                    var username = new StringContent("fileshocker");
                    var password = new StringContent("mikael92");
                    multipartFormContent.Add(bytes, "file", image.FileName);
                    multipartFormContent.Add(username, "username");
                    multipartFormContent.Add(password, "password");
                    var response = await client.PostAsync(new Uri(FileshockerUrl), multipartFormContent);
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