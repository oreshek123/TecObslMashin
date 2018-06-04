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
        public delegate void ProjectStateHandler(string message);
        ProjectStateHandler _del;
       
        public List<Project> Projects;
        public List<Admin> Admins;
        public CarCreate carCreate = new CarCreate();
        public CreateBreakDown CreateBreak = new CreateBreakDown();
        public List<BreakDown> Breaks;
        public CreateComponent CreateComponent = new CreateComponent();
        public CreateUser CreateUser = new CreateUser();
        private XmlSerializer formatter = new XmlSerializer(typeof(List<Project>));
        private XmlSerializer formatterBreaks = new XmlSerializer(typeof(List<BreakDown>));
        XmlSerializer xml = new XmlSerializer(typeof(List<Admin>));
        private Random Rnd = new Random();


        public void RegisterHandler(ProjectStateHandler del)
        {
            _del = del;
        }
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
        public void LoadFromFile(FileInfo projectsFile)
        {
            using (FileStream fs = new FileStream(projectsFile.FullName, FileMode.OpenOrCreate))
            {
                Projects = (List<Project>)formatter.Deserialize(fs);
            }
        }
        public void LoasAdminsFromFile(FileInfo adminPath)
        {
            using (FileStream fs = new FileStream(adminPath.FullName, FileMode.Open))
            {
                Admins = (List<Admin>)xml.Deserialize(fs);
            }
        }
        public void SerializeAdmins(FileInfo adminPath)
        {
            try
            {
                using (FileStream fs = new FileStream(adminPath.FullName, FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, Admins);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void SerializeProj(FileInfo projectsFile)
        {
            try
            {
                using (FileStream fs = new FileStream(projectsFile.FullName, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, Projects);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void SerializeBreaks(FileInfo breaksPath)
        {
            try
            {
                using (FileStream fs = new FileStream(breaksPath.FullName, FileMode.OpenOrCreate))
                {
                    formatterBreaks.Serialize(fs, Breaks);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void LoadBreakDownsFromFile(FileInfo breaksPath)
        {
            if (File.Exists(breaksPath.FullName))
            {
                using (FileStream fs = new FileStream(breaksPath.FullName, FileMode.OpenOrCreate))
                {
                    Breaks = (List<BreakDown>)formatterBreaks.Deserialize(fs);
                }
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
            else _del("Список пуст");
        }
        public void ShowAllCarsInProject(ref Project project)
        {
            if (project != null)
            {
                foreach (Car item in project.Cars)
                {
                    item.PrintInfo();
                }
            }
            else _del("Список пуст");
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
        public void CreateBreaks(ref Car car, ref Project project)
        {
            
            if (Breaks != null)
            {

                BreakDown brekd = new BreakDown();
                brekd = CreateBreak.Create(ref car, ref project);
                if (brekd != null)
                {
                    car.Active = false;
                    Breaks.Add(brekd);
                    _del("Машина остановлена");
                }
            }
            else
            {
                Breaks = new List<BreakDown>();
                BreakDown brekd = new BreakDown();
                brekd = CreateBreak.Create(ref car, ref project);
                if (brekd != null)
                {
                    car.Active = false;
                    Breaks.Add(brekd);
                    _del("Машина остановлена");
                }
            }
        }
        public void CreateBreaks(ref Car car, ref Project project, ref User user)
        {
            if (Breaks != null)
            {
                BreakDown brekd = new BreakDown();
                brekd = CreateBreak.Create(ref car, ref project, ref user);
                if (brekd != null)
                {
                    car.Active = false;
                    Breaks.Add(brekd);
                    _del("Машина остановлена");
                }
            }
            else
            {
                Breaks = new List<BreakDown>();
                BreakDown brekd = new BreakDown();
                brekd = CreateBreak.Create(ref car, ref project, ref user);
                if (brekd != null)
                {
                    car.Active = false;
                    Breaks.Add(brekd);
                    _del("Машина остановлена");
                }
            }
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
        public void ShowAllBreaks(ref Project project)
        {
            if (Breaks != null)
            {
                foreach (BreakDown item in Breaks)
                {
                    if (item.Project.Name == project.Name)
                        item.PrintBreakDown();
                }
            }
            else _del("Список пуст");
        }
    }


}
