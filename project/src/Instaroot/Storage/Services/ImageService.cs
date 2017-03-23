using System;
using System.Collections.Generic;
using System.Text;
using Common.Models;
using Storage.Database;
using System.Threading.Tasks;

namespace Storage.Services
{
    public class ImageService : IImageService
    {
        InstarootContext _context;
        public ImageService(InstarootContext context)
        {
            _context = context;
        }
        public async Task<Image> GetImage(int id)
        {
            return await _context.Images.FindAsync(id);
        }

        public async Task<IEnumerable<Image>> GetImages()
        {
            return await Task.FromResult(_context.Images);
        }

        public async Task PostImage(Image image)
        {
            if (image == null)
                return;
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
        }

        public async Task PutImage(Image image)
        {
            if (image == null)
                return;
            _context.Images.Update(image);
            await _context.SaveChangesAsync();
        }
    }
}
