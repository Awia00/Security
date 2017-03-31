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
                .Include("Users.User")
                .Include(image => image.Comments)
                .Include("Comments.User")
                .Where(image => image.Owner.Id == userId || image.Users.Any(imageUser => imageUser.UserId == userId && imageUser.ImageId == image.Id)));
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

        public async Task DeleteImage(User user, int imageId)
        {
            if (user?.Id == null)
                throw new ArgumentException("Null image or user");

            var image = await _context.Images.FindAsync(imageId);
            if (image != null)
            {
                if (image.Owner.Id != user.Id)
                    throw new UnauthorizedAccessException();

                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Share(User sharer, int imageId, string shareWithId)
        {
            if (sharer?.Id == null) throw new ArgumentNullException(nameof(sharer));

            var image = await _context.Images.FindAsync(imageId);

            if (image == null) throw new ArgumentException("Unknown imageId", nameof(imageId));

            if (image.Owner.Id != sharer.Id)
                throw new InvalidOperationException("The sharer is not owner of the image");

            var shareWithUser = await _context.Users.FindAsync(shareWithId);

            if (shareWithUser == null) throw new ArgumentException("Unknown userId", nameof(shareWithId));

            _context.ImageUsers.Add(new ImageUser
            {
                Image = image,
                ImageId = image.Id,
                User = shareWithUser,
                UserId = shareWithUser.Id
            });

            await _context.SaveChangesAsync();
        }
        public async Task Unshare(User sharer, int imageId, string sharedWithId)
        {
            if (sharer?.Id == null) throw new ArgumentNullException(nameof(sharer));

            var image = await _context.Images.FindAsync(imageId);

            if (image == null) throw new ArgumentException("Unknown imageId", nameof(imageId));

            if (image.Owner.Id != sharer.Id && sharedWithId != sharer.Id)
                throw new InvalidOperationException("The (un)sharer is not owner of the image or sharedwith person");

            var imageUserRelation = await _context.ImageUsers.SingleOrDefaultAsync(relation => relation.UserId == sharedWithId && relation.ImageId == imageId);

            if (imageUserRelation != null)
            {
                //_context.Comments.RemoveRange(_context.Comments.Include(comment => comment.User).Where(comment => comment.User.Id==sharedWithId && comment.ImageId == imageId));
                _context.ImageUsers.Remove(imageUserRelation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
