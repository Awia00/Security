using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Instaroot.Models;
using Instaroot.Storage.Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Instaroot.Services
{
    public class ImageService : IImageService
    {
        private readonly InstarootContext _context;
        public ImageService(InstarootContext context)
        {
            _context = context;
        }

        private async Task<IQueryable<Image>> GetAccessibleImages(string userId)
        {
            return await Task.FromResult(_context.Images
                .Include(image => image.Owner)
                .Include(image => image.Users)
                .Include(image => image.Comments)
                .Where(image => image.Owner.Id == userId || image.Users.Any(imageUser => imageUser.UserId == userId)));
        }
        public async Task<Image> GetImage(string userId, int id)
        {
            return await (await GetAccessibleImages(userId)).FirstOrDefaultAsync(image => image.Id == id);
        }

        public async Task<IEnumerable<Image>> GetImages(string userId)
        {
            return await GetAccessibleImages(userId);
        }

        public async Task PostImage(Image image)
        {
            if (image?.Owner?.Id == null)
                throw new ArgumentException("Null image or user");
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
        }
    
        public async Task DeleteImage(Image image)
        {
            if (image?.Owner?.Id == null)
                throw new ArgumentException("Null image or user");
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}
