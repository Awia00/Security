using System;
using System.Collections.Generic;
using System.Text;
using Common.Models;
using Storage.Database;
using System.Threading.Tasks;

namespace Instaroot.Services
{
    public class UserService : IUserService
    {
        InstarootContext _context;
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

        public async Task UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
