using System.Collections.Generic;
using System.Threading.Tasks;
using Instaroot.Models;

namespace Instaroot.Storage.Services
{
    public interface IImageService
    {
        Task<IEnumerable<Image>> GetImages();
        Task<Image> GetImage(int id);
        Task PostImage(Image image);
        Task PutImage(Image image);
    }
}
