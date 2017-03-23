using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Common.Models;
using Storage.Database;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            if (comment == null || comment.User != null)
                throw new ArgumentException("Null comment or user");
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task PutComment(Comment comment)
        {
            if (comment == null || comment.User != null)
                throw new ArgumentException("Null comment or user");
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
