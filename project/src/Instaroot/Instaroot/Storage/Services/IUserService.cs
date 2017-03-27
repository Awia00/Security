using System.Collections.Generic;
using System.Threading.Tasks;
using Instaroot.Models;

namespace Instaroot.Storage.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task UpdateUser(User user);
    }
}
