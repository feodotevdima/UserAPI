using Core;
using Core.Interfeses;
using Presistence;
using System.Xml.Linq;

namespace Aplication
{
    public class UserService : IUserService
    {
        public void Add(User user)
        {
            if (user == null) return;
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public void Remove(User user)
        {
            if (user == null) return;
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
        }

        public List<User> GetUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var l = db.Users.ToList();
                return l;
            }            
        }

        public User GetUserById(int id)
        {
            var l=GetUsers();
            var u = l.Where(item => item.Id == id).FirstOrDefault();
            return u;
        }

        public User GetUserByName(string name)
        {
            var l = GetUsers();
            var u = l.Where(item => item.Name.ToLower() == name.ToLower()).FirstOrDefault();
            return u;
        }
    }
}
