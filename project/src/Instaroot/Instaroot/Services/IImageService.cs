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
        Task<int> PostImage(Image image);
        Task DeleteImage(User user, int imageId);
        Task Share(User sharer, int imageId, string shareWithId);
        Task Unshare(User user, int imageId, string sharedWithId);
    }
}
