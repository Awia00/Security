using System.Collections.Generic;
using System.Threading.Tasks;
using Instaroot.Models;
using Instaroot.Storage.Database;

namespace Instaroot.Storage.Services
{
    public class CommentService : ICommentService
    {
        private readonly InstarootContext _context;
        public CommentService(InstarootContext context)
        {
            _context = context;
        }
        public async Task<Comment> GetComment(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetComments()
        {   
            return await Task.FromResult(_context.Comments);
        }

        public async Task PostComment(Comment comment)
        {
            if (comment == null)
                return;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task PutComment(Comment comment)
        {
            if (comment == null)
                return;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
