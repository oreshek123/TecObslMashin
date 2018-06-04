using System;

namespace Test.Classes
{
    [Serializable]
    public class User : AccesLevel
    {
        public void PrintUser()
        {
            Console.WriteLine($"Логин : {Login}\tПароль : {Password}");
        }
    }
}
