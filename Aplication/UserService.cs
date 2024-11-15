using Core;
using Core.Interfeses;
using Presistence;
using System.Xml.Linq;

namespace Aplication
{
    public class UserService : IUserService
    {
        public Boolean Add(User user)
        {
            if (user == null) return false;
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            return true;
        }

        public Boolean Remove(User user)
        {
            if (user == null) return false;
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
            return true;
        }

        public Boolean Update(User user)
        {
            if (user == null) return false;
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Users.Update(user);
                db.SaveChanges();
            }
            return true;
        }

        public List<User> GetUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var l = db.Users.ToList();
                return l;
            }            
        }

        public User? GetUserById(int id)
        {
            User? u = GetUsers().Where(item => item.Id == id).FirstOrDefault();
            return u;
        }

        public User? GetUserByEmail(string email)
        {
            User? u = GetUsers().Where(item => item.Name.ToLower() == email.ToLower()).FirstOrDefault();
            return u;
        }
    }
}
