using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using RandomNameGenerator;
using Test.Classes;

namespace ModuleUser
{
    [Serializable]
    public class CreateUser
    {
        public Random Rnd = new Random();
        public List<User> GenerateUsers()
        {
            List<User> users = new List<User>();
            for (int i = 0; i < Rnd.Next(1, 10); i++)
            {
                User user = new User
                {
                    Login = NameGenerator.GenerateLastName().ToLower(),
                    Password = "123",
                    //Project = projects[Rnd.Next(0, projects.Count)]
                };
                users.Add(user);
            }
            List<Admin> admins = new List<Admin>();
            for (int i = 0; i < Rnd.Next(1, 10); i++)
            {
                Admin admin = new Admin
                {
                    Login = NameGenerator.GenerateLastName().ToLower(),
                    Password = "Admin"
                };
                admins.Add(admin);
            }
            XmlSerializer xmlUser = new XmlSerializer(typeof(List<User>));
            using (FileStream fs = new FileStream("Users.xml", FileMode.OpenOrCreate))
            {
                xmlUser.Serialize(fs, users);
            }

            XmlSerializer xmlAdmin = new XmlSerializer(typeof(List<Admin>));
            using (FileStream fs = new FileStream("Admins.xml", FileMode.OpenOrCreate))
            {
                xmlAdmin.Serialize(fs, admins);
            }

            return users;
        }

        public static void CUser(ref Project project, string login, string password)
        {
            if (project.Users != null)
            {
                bool isUser = false;
                foreach (User item in project.Users)
                {
                    if (item.Login == login)
                        isUser = true;
                }

                if (!isUser)
                {
                    User user = new User
                    {
                        Login = login,
                        Password = password
                    };
                    project.Users.Add(user);
                    Console.WriteLine("Пользователь успешно создан");
                }
                else Console.WriteLine("Пользователь уже существует на этом проекте");

            }
            else
            {
                project.Users = new List<User>();
                User user = new User
                {
                    Login = login,
                    Password = password
                };
                project.Users.Add(user);

            }
        }
    }
}
