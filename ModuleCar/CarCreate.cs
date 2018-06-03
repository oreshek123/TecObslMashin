using System;
using System.Collections.Generic;
using RandomNameGenerator;

using Test.Classes;
using ModuleComponent;

namespace ModuleCar
{
    public class CarCreate
    {
        private Random Rnd = new Random();
        public CreateComponent CreateComponent = new CreateComponent();
        private readonly Char[] _pwdChars = new Char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
            'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public List<Car> CreateCar()
        {
            List<Car> Cars = new List<Car>();
            for (int i = 0; i < Rnd.Next(1, 10); i++)
            {
                Car car = new Car()
                {
                    Components = CreateComponent.GenerateComponents(),
                    
                    GarageNuber = $"{_pwdChars[Rnd.Next(0,_pwdChars.Length)]}" +
                                  $"{Rnd.Next(0,10)}" +
                                  $"{Rnd.Next(0, 10)}" +
                                  $"{Rnd.Next(0, 10)}" +
                                  $"{_pwdChars[Rnd.Next(0, _pwdChars.Length)]}" +
                                  $"{_pwdChars[Rnd.Next(0, _pwdChars.Length)]}" +
                                  $"{_pwdChars[Rnd.Next(0, _pwdChars.Length)]}",
                    Model = NameGenerator.GenerateLastName().ToLower(),
                    Name = NameGenerator.GenerateFirstName((Gender)Rnd.Next(0, 2)).ToLower(),
                    TypeOfCar = (TypeOfCar)Rnd.Next(0, 2),
                    YearOfIssue = Rnd.Next(1950, 2019)
                };
                Cars.Add(car);
            }
            return Cars;
        }

        public static void CreateCar(ref Project project)
        {
            Console.WriteLine("Введите название машины");
            string name = Console.ReadLine();
            Console.WriteLine("Введите модель машины");
            string model = Console.ReadLine();
            Console.WriteLine("Введите гаражный номер машины");
            string garageNumber = Console.ReadLine().ToUpper();
            Console.WriteLine("Введите тип машины 0 - механика 1 - автомат");
            int.TryParse(Console.ReadLine(), out int type);
            TypeOfCar t = (TypeOfCar)type;
            Console.WriteLine("Введите год выпуска машины");
            int.TryParse(Console.ReadLine(), out int yearOfIssue);
            if (project.Cars != null)
            {
                Car car = new Car()
                {
                    Name = name,
                    GarageNuber = garageNumber,
                    Model = model,
                    YearOfIssue = yearOfIssue,
                    TypeOfCar = t,
                };
                project.Cars.Add(car);
            }
        }
        public static bool IsComponentInThatCar(ref Car car,out int id)
        {
            Console.WriteLine("Введите id компонента");
            int.TryParse(Console.ReadLine(), out int idcomp);
            Part partc = new Part()
            {
                ComponentId = idcomp
            };
            bool res = false;
            if (car.Components != null)
            {
                foreach (Part part in car.Components)
                {
                    if (part.ComponentId == partc.ComponentId)
                        res = true;
                }
            }

            id = idcomp;
            return res;
        } 
        public static void AttachComponentToCar(ref Car car, ref Part component, out string message)
        {
            string mes = "";
            if (car.Components != null)
            {
                foreach (Part item in car.Components)
                {
                    if (component != item)
                    {
                        car.Components.Add(component);
                        mes = "Компонент добавлен успешно";
                    }
                    else mes = $"Данный компонент уже есть в этой машине";
                    break;
                }
            }
            else
            {
                car.Components = new List<Part>();
                car.Components.Add(component);
                mes = "Компонент добавлен успешно";
            }
            message = mes;
        }

        public static void GetCarActiveOrNot(ref Car car, bool sost)
        {
            if (car != null) car.Active = sost;
        }
    }
}
