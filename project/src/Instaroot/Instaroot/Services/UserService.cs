using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Instaroot.Models;
using Instaroot.Storage.Database;

namespace Instaroot.Services
{
    public class UserService : IUserService
    {
        private readonly InstarootContext _context;
        public UserService(InstarootContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await Task.FromResult(_context.Users);
        }

        public Task UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
