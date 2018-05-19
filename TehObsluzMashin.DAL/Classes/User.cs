using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TehObsluzMashin.DAL.Classes
{
    public enum AccesLevel
    {
        Admin, User
    }
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public AccesLevel AccesLevel { get; set; }

        public void PrintUser()
        {
            Console.WriteLine($"Логин : {Login}\tПароль : {Password}\tУровень доступа : {AccesLevel}");
        }
    }
}
