using System;
using System.Collections.Generic;

namespace Test.Classes
{
    [Serializable]
    public class Project
    {
        public string Name { get; set; }

        public List<Car> Cars { get; set; }
        public List<User> Users { get; set; }

        public void PrintProject()
        {
            Console.WriteLine("--------------------Проект------------------");
            Console.WriteLine($"Наименование проекта : {Name}");
            foreach (Car car in Cars)
            {
                car.PrintInfo();
            }

        }
    }
}
