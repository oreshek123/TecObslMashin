using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ModuleBreakDown;
using ModuleCar;
using ModuleComponent;
using ModuleUser;
using RandomNameGenerator;
using Test.Classes;


namespace TehObsluzMashin.DAL
{
    [Serializable]
    public class CreateProject
    {
        public List<Project> Projects;
        public CarCreate carCreate = new CarCreate();
        public CreateBreakDown CreateBreak = new CreateBreakDown();
        public List<BreakDown> Breaks;
        public CreateComponent CreateComponent = new CreateComponent();
        public CreateUser CreateUser = new CreateUser();
        private XmlSerializer formatter = new XmlSerializer(typeof(List<Project>));
      

        private Random Rnd = new Random();
        public void GenerateProjects()
        {
            Projects = new List<Project>();

            for (int i = 0; i < Rnd.Next(1, 10); i++)
            {
                Project project = new Project()
                {
                    Cars = carCreate.CreateCar(),
                    Name = NameGenerator.GenerateFirstName((Gender)Rnd.Next(0, 2)).ToLower(),
                    Users = CreateUser.GenerateUsers()
                };
                Projects.Add(project);
            }

            XmlSerializer formatter = new XmlSerializer(typeof(List<Project>));
            using (FileStream fs = new FileStream("Projects.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Projects);
            }
        }

        public void LoadFromFile()
        {

            using (FileStream fs = new FileStream("Projects.xml", FileMode.OpenOrCreate))
            {
                Projects = (List<Project>)formatter.Deserialize(fs);
            }
        }

        public void SerializeProj()
        {
            try
            {
                using (FileStream fs = new FileStream("Projects.xml", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, Projects);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void CreateProj()
        {
            Console.WriteLine("Введите имя проекта");
            string name = Console.ReadLine();

            Project project = new Project()
            {
                Name = name,
                Cars = new List<Car>()
            };
            Projects.Add(project);
        }
        public void ShowAllCarsInProject(ref List<Project> projects)
        {
            if (projects != null)
            {
                foreach (Project pro in projects)
                {
                    pro.PrintProject();
                }
            }
        }

        public Project SearchProject(ref List<Project> projects, string name)
        {
            Project project = new Project();
            if (projects != null)
            {
                foreach (Project item in projects)
                {
                    if (item.Name == name)
                        project = item;
                }
            }

            return project;
        }
        public Car SerchCarByNumber(out string message, ref Project project, string garageNumberOrModel)
        {
            Car autoCar = new Car();
            int i = 0;
            bool IsTrue = false;
            if (project.Cars != null)
            {
                foreach (Car car in project.Cars)
                {
                    if (car.GarageNuber == garageNumberOrModel || car.Model == garageNumberOrModel)
                    {
                        autoCar = car;
                        IsTrue = true;
                        car.PrintInfo();
                        i++;
                    }
                }
            }
            if (IsTrue == false)
                autoCar = null;

            message = $"Найдено {i} машин";
            return autoCar;
        }
        public bool IsBroken(ref Car car)
        {
            bool result = false;

            if (Breaks != null)
            {
                foreach (BreakDown item in Breaks)
                {
                    if (item.Car == car)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }
        public void CreateBreaks(ref Car car, out string message, ref Project project)
        {
            Console.WriteLine("Введите описание поломки");
            string description = Console.ReadLine();
            Console.WriteLine("Введите рекомендации по починке");
            string recommends = Console.ReadLine();
            user:
            Console.WriteLine("Введите логин пользователя, который проводил осмотр ");
            string login = Console.ReadLine();
            User user = new User();
            string mes = "";
            if (project.Users != null)
            {
                bool isUser = false;
                foreach (User us in project.Users)
                {
                    if (us.Login == login)
                    {
                        user = us;
                        isUser = true;
                    }
                }

                if (isUser == false)
                {
                    Console.WriteLine("Такого пользователя не существует в этом проекте");
                    goto user;
                }
            }
            if (Breaks != null)
            {

                BreakDown brekd = new BreakDown();
                brekd = CreateBreak.Create(ref car, description, recommends, ref user);
                car.Active = false;
                Breaks.Add(brekd);
                mes = $"Машина остановлена";
            }
            else
            {
                Breaks = new List<BreakDown>();
                BreakDown brekd = new BreakDown();
                brekd = CreateBreak.Create(ref car, description, recommends, ref user);
                car.Active = false;
                Breaks.Add(brekd);
                mes = $"Машина остановлена";
            }

            message = mes;
        }
        public void ShowAllBreaks()
        {
            if (Breaks != null)
            {
                foreach (BreakDown breakDown in Breaks)
                {
                    breakDown.PrintBreakDown();
                }
            }
        }
    }


}
