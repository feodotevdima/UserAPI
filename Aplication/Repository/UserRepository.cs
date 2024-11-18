using Core;
using Core.Interfeses;
using Microsoft.EntityFrameworkCore;
using Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> AddUserAsync(User user)
        {
            if (user == null) return null;
            using (ApplicationContext db = new ApplicationContext())
            {
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
            }
            return user;
        }

        public async Task<User> RemoveUserAsync(Guid id)
        {
            var user = await GetUserByIdAsync(id);
            if (user == null) return null;
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            if (user == null) return null;
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Users.Update(user);
                await db.SaveChangesAsync();
            }
            return user;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var users = await db.Users.ToListAsync();
                return users;
            }
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            List<User> users = await GetUsersAsync();
            var user = users.FirstOrDefault(item => item.Id == id);
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            List<User> users = await GetUsersAsync();
            var user = users.FirstOrDefault(item => item.Email.ToLower() == email.ToLower());
            return user;
        }
    }
}
