using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Instaroot.Storage.Database;
using Instaroot.Models;

namespace Instaroot.Services
{
    public class CommentService : ICommentService
    {
        private readonly InstarootContext _context;

        public CommentService(InstarootContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetComments(string userId)
        {
            return await Task.FromResult(_context.Comments.Include(comment => comment.User).Where(comment => comment.User.Id == userId));
        }

        public async Task PostComment(Comment comment)
        {
            if (comment?.User?.Id == null || comment.ImageId == 0)
                throw new ArgumentException("Null comment, user or imageId");
            if (_context.ImageUsers.Any(imageUser => imageUser.UserId == comment.User.Id && comment.ImageId == imageUser.ImageId))
            {
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task PutComment(Comment comment)
        {
            if (comment?.User?.Id == null || comment.ImageId == 0)
                throw new ArgumentException("Null comment, user or imageId");
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteComment(Comment comment)
        {
            if (comment?.User?.Id == null || comment.ImageId == 0)
                throw new ArgumentException("Null comment, user or imageId");
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
