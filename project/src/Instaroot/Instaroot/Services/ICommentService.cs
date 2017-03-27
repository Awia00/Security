using Instaroot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Instaroot.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetComments(string userId);
        Task PostComment(Comment comment);
        Task PutComment(Comment comment);
    }
}
