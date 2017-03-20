using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUser(int id);
        void PostUser(User user);
        void UpdateUser(User user);
    }
}
