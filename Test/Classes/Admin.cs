using System;

namespace Test.Classes
{
    [Serializable]
    public class Admin: IAccesLevel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        
        public void PrintAdmin()
        {
            Console.WriteLine($"Логин : {Login}\tПароль : {Password}\t");
        }
    }
}