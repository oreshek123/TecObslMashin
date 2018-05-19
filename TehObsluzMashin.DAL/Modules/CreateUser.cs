using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomNameGenerator;
using TehObsluzMashin.DAL.Classes;

namespace TehObsluzMashin.DAL.Modules
{
    public class CreateUser
    {
        private Random Rnd = new Random();
        public List<User> GenerateUsers()
        {
            List<User> users = new List<User>();
            for (int i = 0; i < Rnd.Next(1, 10); i++)
            {
                User user = new User()
                {
                    Login = NameGenerator.GenerateLastName().ToLower(),
                    Password = "123",
                    AccesLevel = AccesLevel.User
                };
                users.Add(user);
            }

            User admin = new User()
            {
                AccesLevel = AccesLevel.Admin,
                Login = "Admin",
                Password = "Admin"
            };
            users.Add(admin);

            return users;
        }

        public static void CUser(ref Project project, string login, string password)
        {
            if (project.Users != null)
            {
                User user = new User()
                {
                    AccesLevel = AccesLevel.User,
                    Login = login,
                    Password = password
                };
                project.Users.Add(user);
            }
            else
            {
                project.Users = new List<User>();
                User user = new User()
                {
                    AccesLevel = AccesLevel.User,
                    Login = login,
                    Password = password
                };
                User admin = new User()
                {
                    AccesLevel = AccesLevel.Admin,
                    Login = "Admin",
                    Password = "Admin"
                };
                project.Users.Add(user);
                project.Users.Add(admin);
            }
        }
    }
}
