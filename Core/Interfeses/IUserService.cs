using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfeses
{
    public interface IUserService
    {
        Boolean Add(User user);
        Boolean Remove(User user);
        Boolean Update(User user);
        List<User> GetUsers();
        User? GetUserById(int id);
        User? GetUserByEmail(string email);
    }
}
