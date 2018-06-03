using System;

namespace Test.Classes
{
    [Serializable]
    public class Admin: IAccesLevel
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public Admin()
        {
            
        }
        public Admin(string login, string password)
        {
            Login = login;
            Password = password;
        }
        public void PrintAdmin()
        {
            Console.WriteLine($"Логин : {Login}\tПароль : {Password}\t");
        }
    }
}