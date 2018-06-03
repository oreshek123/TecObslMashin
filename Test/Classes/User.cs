using System;

namespace Test.Classes
{
    [Serializable]
    public class User : IAccesLevel
    {
        public Project Project { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public void PrintUser()
        {
            Console.WriteLine($"Логин : {Login}\tПароль : {Password}\tПроект: {Project.Name}");
        }
    }
}
