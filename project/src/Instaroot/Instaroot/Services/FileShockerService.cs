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
        private readonly string _username;
        private readonly string _password;
        private readonly string _fileshockerAddress;
        private readonly ILoggingService _loggingService;

        public FileShockerService(string username, string password, string fileshockerAddress, ILoggingService loggingService)
        {
            _username = username;
            _password = password;
            _loggingService = loggingService;
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
                    var username = new StringContent(_username);
                    var password = new StringContent(_password);
                    multipartFormContent.Add(bytes, "file", image.FileName);
                    multipartFormContent.Add(username, "username");
                    multipartFormContent.Add(password, "password");
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