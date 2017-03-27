using Instaroot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Instaroot.Services
{
    public interface IImageService
    {
        Task<IEnumerable<Image>> GetImages(string userId);
        Task<Image> GetImage(string userId, int id);
        Task PostImage(Image image);
        Task DeleteImage(Image image);
    }
}
