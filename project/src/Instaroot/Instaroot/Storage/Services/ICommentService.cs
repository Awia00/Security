using System.Collections.Generic;
using System.Threading.Tasks;
using Instaroot.Models;

namespace Instaroot.Storage.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetComments();
        Task<Comment> GetComment(int id);
        Task PostComment(Comment comment);
        Task PutComment(Comment comment);
    }
}
