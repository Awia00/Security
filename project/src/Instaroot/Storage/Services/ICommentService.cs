using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Services
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetComments();
        Comment GetComment(int id);
        void PostComment(Comment comment);
        void PutComment(Comment comment);
    }
}
