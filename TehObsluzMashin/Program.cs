﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ModuleCar;
using ModuleComponent;
using ModuleUser;
using TehObsluzMashin.DAL;
using Test.Classes;


namespace TehObsluzMashin
{
    class Program
    {  
        static DirectoryInfo path = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent?.Parent?.Parent;
        static FileInfo adminPath = new FileInfo(path.FullName + "/TehObsluzMashin.DAL/Data/Admins.xml");
        static FileInfo projectPath = new FileInfo(path.FullName + "/TehObsluzMashin.DAL/Data/Projects.xml");
        static FileInfo breaksPath = new FileInfo(path.FullName + "/TehObsluzMashin.DAL/Data/BreakDowns.xml");
        static void Main(string[] args)
        {
            try
            {
                CreateProject createProject = new CreateProject();
                createProject.RegisterHandler(Show_Message);
                createProject.LoadFromFile(projectPath);
                createProject.LoadBreakDownsFromFile(breaksPath);
                createProject.LoasAdminsFromFile(adminPath);
                start:
                Console.Clear();
                User user = null;
                Project proj = null;
                Console.WriteLine("1 - Администратор\n2 - Пользователь\n3 - Зарегистрироваться");
                Int32.TryParse(Console.ReadLine(), out int IsAdmin);
                switch (IsAdmin)
                {
                    case 1:
                    {
                        if (!LoginAdmin(createProject))
                        {
                            Console.WriteLine("Good Bye");
                            Thread.Sleep(3000);
                            return;
                        }

                        AdminMenu(createProject);
                        break;
                    }
                    case 2:
                    {
                        int countOfIncorrectChance = 3;
                            UserMenu:
                        Console.WriteLine("Введите логин");
                        string login = Console.ReadLine();
                        Console.WriteLine("Введите пароль");
                        string password = Console.ReadLine();
                        foreach (Project item in createProject.Projects)
                        {
                            foreach (User itemUser in item.Users)
                            {
                                if (itemUser.Login == login && itemUser.Password == password)
                                {
                                    user = itemUser;
                                    proj = item;

                                    break;
                                }
                            }
                        }

                        if (user != null)
                        {
                            Console.WriteLine("Добро пожаловать");
                            Thread.Sleep(1000);
                            Console.Clear();
                            UserMenu(proj, user, createProject);

                        }
                        else if (countOfIncorrectChance > 1)
                        {
                            Console.WriteLine("Ваши данные некорректны!");
                            countOfIncorrectChance--;
                            Console.WriteLine("Осталось попыток : " + countOfIncorrectChance);
                            Thread.Sleep(2000);
                            Console.Clear();
                            goto UserMenu;
                        }
                        else
                            return;

                        break;
                    }
                    case 3:
                    {
                        Console.WriteLine("Введите логин");
                        string login = Console.ReadLine();
                        Console.WriteLine("Введите пароль");
                        string password = Console.ReadLine();

                        createProject.Admins.Add(new Admin(login, password));


                        break;

                    }
                }



                createProject.SerializeProj(projectPath);
                createProject.SerializeBreaks(breaksPath);
                createProject.SerializeAdmins(adminPath);
                Console.WriteLine("Для выхода в главное меню нажмите букву \"M\"");
                if (Console.ReadKey().Key == ConsoleKey.M)
                    goto start;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }
        private static void Show_Message(String message)
        {
            Console.WriteLine(message);
        }
        static void UserMenu(Project proj, User user, CreateProject createProject)
        {
            start:
            Console.Clear();
            Console.WriteLine("1 - Машина\n2 - Компонент\n3 - Останов");
            Int32.TryParse(Console.ReadLine(), out int userChoice);
            switch (userChoice)
            {
                case 1:
                    {
                        Console.Clear();
                        Console.WriteLine("------------------------Контроль технического обслуживания машин-------------------------");
                        Console.WriteLine("1 - Отображение всего парка машин на проекте\n" +
                                          "2 - Поиск машины по его гаражному номеру и модели\n" +
                                          "3 - Сделать останов машины\n" +
                                          "4 - Прикрепить компонент к машине\n" +
                                          "5 - Сделать машину активной/неактивной\n" +
                                          "6 - Создать машину");
                        Console.WriteLine("Вернуться в меню - enter x2");
                        Int32.TryParse(Console.ReadLine(), out int choice);
                        switch (choice)
                        {
                            case 1:
                                {
                                    createProject.ShowAllCarsInProject(ref proj);
                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                        goto start;
                                    break;
                                }
                            case 2:
                                {
                                    SearchCar(ref proj);
                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                    {
                                        goto start;
                                    }

                                    break;
                                }
                            case 3:
                                {
                                    Car car = SearchCarForStop(ref proj);
                                    if (car != null)
                                    {
                                        if (createProject.IsBroken(ref car))
                                            Console.WriteLine("Машина уже не в рабочем состоянии");
                                        else
                                        {
                                            createProject.CreateBreaks(ref car, ref proj, ref user);
                                        }
                                    }
                                    else Console.WriteLine("Проверьте корректность ввода");

                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                    {
                                        goto start;
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    Car car = SearchCar(ref proj);
                                    if (car != null)
                                    {
                                        if (CarCreate.IsComponentInThatCar(ref car, out int id))
                                        {
                                            Console.WriteLine("Компонент уже есть в данной машине");
                                            Console.WriteLine("Создать новый компонент 1 - да Любая клавиша - выход в меню");
                                            Int32.TryParse(Console.ReadLine(), out int ch);
                                            if (ch == 1)
                                            {
                                                Console.WriteLine("Введите id компонента");
                                                Int32.TryParse(Console.ReadLine(), out id);
                                                Part component = CreateComponent.CtComponent(id);
                                                CarCreate.AttachComponentToCar(ref car, ref component, out string message);
                                                Console.WriteLine(message);
                                            }
                                            else goto start;
                                        }
                                        else
                                        {
                                            Part component = CreateComponent.CtComponent(id);
                                            CarCreate.AttachComponentToCar(ref car, ref component, out string message);
                                            Console.WriteLine(message);
                                        }
                                    }
                                    else Console.WriteLine("Проверьте корректность ввода");

                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                        goto start;
                                    break;
                                }
                            case 5:
                                {
                                    Car car = SearchCar(ref proj);
                                    if (car != null)
                                    {
                                        Console.WriteLine("1 - Активной 2 - Неактивной");


                                        Int32.TryParse(Console.ReadLine(), out int v);
                                        switch (v)
                                        {
                                            case 1:
                                            {
                                                CarCreate.GetCarActiveOrNot(ref car, true);
                                                Console.WriteLine("Машина активна");
                                                break;
                                            }
                                            case 2:
                                            {
                                                CarCreate.GetCarActiveOrNot(ref car, false);
                                                Console.WriteLine("Машина неактивна");
                                                break;
                                            }
                                        }
                                    }
                                    else Console.WriteLine("Проверьте корректность ввода");

                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                        goto start;

                                    break;
                                }
                            case 6:
                                {
                                    CarCreate.CreateCar(ref proj);
                                    Console.WriteLine("Машина добавлена в список машин на проекте");

                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                        goto start;
                                    break;
                                }
                        }
                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                            goto start;
                        break;
                    }
                case 2:
                    {
                        Console.Clear();
                        Console.WriteLine("------------------------Контроль технического обслуживания машин-------------------------");
                        Console.WriteLine("1 - Отображение всех компонентов на проекте\n" +
                                          "2 - Создать компонент и прикрепить его к машине\n" +
                                          "Вернуться в меню - enter x2");

                        Int32.TryParse(Console.ReadLine(), out int c);
                        switch (c)
                        {
                            case 1:
                                {
                                    foreach (Car item in proj.Cars)
                                        item.PrintOnlyComponents();
                                    break;
                                }
                            case 2:
                                {
                                    Console.WriteLine("Введите id компонента");
                                    Int32.TryParse(Console.ReadLine(), out int idco);
                                    Part part = CreateComponent.CtComponent(idco);
                                    Car car = SearchCar(ref proj);
                                    if (car != null)
                                    {
                                        CarCreate.AttachComponentToCar(ref car, ref part, out var mes);
                                        Console.WriteLine(mes);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Модель или гаражный номер введены неверно, либо данная машина не пренадлежит проекту");
                                    }

                                    break;
                                }

                        }

                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                            goto start;

                        break;
                    }
                case 3:
                    {
                        Console.Clear();
                        Console.WriteLine("------------------------Контроль технического обслуживания машин-------------------------");
                        Console.WriteLine("1 - Отобразить весь список остановов на проекте\n" +
                                          "2 - Создать останов для доступной машины\n" +
                                          "Вернуться в меню - enter x2");
                        Int32.TryParse(Console.ReadLine(), out int cl);
                        switch (cl)
                        {
                            case 1:
                                {
                                    createProject.ShowAllBreaks(ref proj);
                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                    {
                                        goto start;
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    Car car = SearchCarForStop(ref proj);
                                    if (car != null)
                                    {
                                        if (createProject.IsBroken(ref car))
                                            Console.WriteLine("Машина уже не в рабочем состоянии");
                                        else
                                        {
                                            createProject.CreateBreaks(ref car, ref proj, ref user);
                                        }
                                    }
                                    else Console.WriteLine("Проверьте корректность ввода");

                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                    {
                                        goto start;
                                    }
                                    break;


                                }
                        }

                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                            goto start;

                        break;

                    }



            }
        }
        static void AdminMenu(CreateProject createProject)
        {
            start:
            Console.Clear();
            Console.WriteLine("------------------------Контроль технического обслуживания машин-------------------------");
            Console.WriteLine("1 - Машина\n2 - Компонент\n3 - Проект\n4 - Останов\n5 - Пользователь");
            int cho = 0;
            Int32.TryParse(Console.ReadLine(), out cho);
            switch (cho)
            {
                case 1:
                    {
                        Console.Clear();
                        Console.WriteLine("------------------------Контроль технического обслуживания машин-------------------------");
                        Console.WriteLine("1 - Отображение всего парка машин по проектам\n" +
                                          "2 - Поиск машины по его гаражному номеру и модели\n" +
                                          "3 - Сделать останов машины\n" +
                                          "4 - Прикрепить компонент к машине\n" +
                                          "5 - Сделать машину активной/неактивной\n" +
                                          "6 - Создать машину");
                        Console.WriteLine("Вернуться в меню - enter x2");
                        Int32.TryParse(Console.ReadLine(), out int choice);
                        switch (choice)
                        {
                            case 1:
                                {
                                    createProject.ShowAllCarsInProject(ref createProject.Projects);
                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                        goto start;
                                    break;
                                }
                            case 2:
                                {
                                    SearchCar(ref createProject);
                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                    {
                                        goto start;
                                    }

                                    break;
                                }
                            case 3:
                                {
                                    Car car = SearchCar(ref createProject, out Project project);
                                    if (createProject.IsBroken(ref car))
                                        Console.WriteLine("Машина уже не в рабочем состоянии");
                                    else
                                    {
                                        if (car != null)
                                            createProject.CreateBreaks(ref car, ref project);
                                        else Console.WriteLine("Проверьте корректность ввода");
                                    }

                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                    {
                                        goto start;
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    Car car = SearchCar(ref createProject);
                                    if (car != null)
                                    {
                                        if (CarCreate.IsComponentInThatCar(ref car, out int id))
                                        {
                                            Console.WriteLine("Компонент уже есть в данной машине");
                                            Console.WriteLine("Создать новый компонент 1 - да Любая клавиша - выход в меню");
                                            Int32.TryParse(Console.ReadLine(), out int ch);
                                            if (ch == 1)
                                            {
                                                Console.WriteLine("Введите id компонента");
                                                Int32.TryParse(Console.ReadLine(), out id);
                                                Part component = CreateComponent.CtComponent(id);
                                                CarCreate.AttachComponentToCar(ref car, ref component, out string message);
                                                Console.WriteLine(message);
                                            }
                                            else goto start;
                                        }
                                        else
                                        {
                                            Part component = CreateComponent.CtComponent(id);
                                            CarCreate.AttachComponentToCar(ref car, ref component, out string message);
                                            Console.WriteLine(message);
                                        }
                                    }
                                    else Console.WriteLine("Проверьте корректность ввода");

                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                        goto start;
                                    break;
                                }
                            case 5:
                                {
                                    Car car = SearchCar(ref createProject);
                                    if (car != null)
                                    {
                                        Console.WriteLine("1 - Активной 2 - Неактивной");
                                        Int32.TryParse(Console.ReadLine(), out int v);


                                        switch (v)
                                        {
                                            case 1:
                                                {
                                                    CarCreate.GetCarActiveOrNot(ref car, true);
                                                    Console.WriteLine("Машина активна");
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    CarCreate.GetCarActiveOrNot(ref car, false);
                                                    Console.WriteLine("Машина неактивна");
                                                    break;
                                                }
                                        }
                                    }
                                    else Console.WriteLine("Проверьте корректность ввода");

                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                        goto start;

                                    break;
                                }
                            case 6:
                                {
                                    Console.WriteLine("Введите название проекта");
                                    int i = 0;
                                    foreach (Project item in createProject.Projects)
                                    {
                                        Console.WriteLine($"{++i} {item.Name}");
                                    }
                                    string progname = Console.ReadLine();
                                    Project project = createProject.SearchProject(ref createProject.Projects, progname);
                                    if (project.Name != null)
                                    {
                                        CarCreate.CreateCar(ref project);
                                        Console.WriteLine("Машина добавлена в список машин на проекте");
                                    }
                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                        goto start;
                                    break;
                                }
                        }
                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                            goto start;
                        break;
                    }
                case 2:
                    {
                        Console.Clear();
                        Console.WriteLine("------------------------Контроль технического обслуживания машин-------------------------");
                        Console.WriteLine("1 - Отображение всех компонентов на проекте\n" +
                                          "2 - Создать компонент и прикрепить его к машине\n" +
                                          "Вернуться в меню - enter x2");

                        Int32.TryParse(Console.ReadLine(), out int c);
                        switch (c)
                        {
                            case 1:
                                {

                                    int i = 0;
                                    Console.WriteLine("Введите название проекта");
                                    foreach (Project item in createProject.Projects)
                                        Console.WriteLine($"{++i} {item.Name}");

                                    string name = Console.ReadLine();
                                    Project project = createProject.SearchProject(ref createProject.Projects, name);
                                    if (project.Name != null)
                                        foreach (Car item in project.Cars)
                                            item.PrintOnlyComponents();
                                    break;
                                }
                            case 2:
                                {
                                    Console.WriteLine("Введите id компонента");
                                    Int32.TryParse(Console.ReadLine(), out int idco);
                                    Part part = CreateComponent.CtComponent(idco);
                                    Car car = SearchCar(ref createProject);
                                    if (car != null)
                                    {
                                        CarCreate.AttachComponentToCar(ref car, ref part, out var mes);
                                        Console.WriteLine(mes);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Модель или гаражный номер введены неверно, либо данная машина не пренадлежит проекту");
                                    }

                                    break;
                                }

                        }

                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                            goto start;

                        break;
                    }
                case 3:
                    {
                        Console.Clear();
                        Console.WriteLine("------------------------Контроль технического обслуживания машин-------------------------");
                        Console.WriteLine("1 - Отобразить весь список проектов\n" +
                                          "2 - Создать проект\n" +
                                          "Вернуться в меню - enter x2");
                        Int32.TryParse(Console.ReadLine(), out int b);
                        switch (b)
                        {
                            case 1:
                                {
                                    int i = 0;
                                    Console.WriteLine("Все проекты");
                                    foreach (Project item in createProject.Projects)
                                    {
                                        Console.WriteLine($"{++i} {item.Name}");
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    createProject.CreateProj();
                                    Console.WriteLine("Проект успешно создан и добавлен в список");
                                    break;
                                }
                        }

                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                            goto start;
                        break;
                    }
                case 5:
                    {
                        Console.Clear();
                        Console.WriteLine("------------------------Контроль технического обслуживания машин-------------------------");
                        Console.WriteLine("1 - Отобразить весь список пользователей по проектам\n" +
                                          "2 - Создать пользователя\n" +
                                          "Вернуться в меню - enter x2 ");
                        Int32.TryParse(Console.ReadLine(), out int us);
                        switch (us)
                        {
                            case 1:
                                {
                                    int i = 0;
                                    Console.WriteLine("Введите название проекта");
                                    foreach (Project item in createProject.Projects)
                                        Console.WriteLine($"{++i} {item.Name}");
                                    string name = Console.ReadLine();
                                    Project project = createProject.SearchProject(ref createProject.Projects, name);
                                    if (project.Name != null)
                                    {
                                        foreach (User item in project.Users)
                                        {
                                            item.PrintUser();
                                        }
                                    }

                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                        goto start;
                                    break;
                                }
                            case 2:
                                {
                                    int i = 0;
                                    Console.WriteLine("Введите название проекта");
                                    foreach (Project item in createProject.Projects)
                                        Console.WriteLine($"{++i} {item.Name}");
                                    string name = Console.ReadLine();
                                    Project project = createProject.SearchProject(ref createProject.Projects, name);
                                    if (project.Name != null)
                                    {
                                        Console.WriteLine("Введите логин");
                                        string login = Console.ReadLine();
                                        Console.WriteLine("Введите пароль");
                                        string password = Console.ReadLine();
                                        CreateUser.CUser(ref project, login, password);

                                    }

                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                        goto start;
                                    break;
                                }
                        }

                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                            goto start;
                        break;
                    }
                case 4:
                    {
                        Console.Clear();
                        Console.WriteLine("------------------------Контроль технического обслуживания машин-------------------------");
                        Console.WriteLine("1 - Отобразить весь список остановов\n" +
                                          "2 - Создать останов для доступной машины\n" +
                                          "Вернуться в меню - enter x2");
                        Int32.TryParse(Console.ReadLine(), out int cl);
                        switch (cl)
                        {
                            case 1:
                                {
                                    createProject.ShowAllBreaks();
                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                    {
                                        goto start;
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    Car car = SearchCar(ref createProject, out Project project);
                                    if (car != null)
                                    {
                                        if (createProject.IsBroken(ref car))
                                            Console.WriteLine("Машина уже не в рабочем состоянии");
                                        else
                                        {
                                            createProject.CreateBreaks(ref car, ref project);
                                        }
                                    }
                                    else Console.WriteLine("Проверьте корректность ввода");

                                    Console.WriteLine("Вернуться в меню - enter");
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                    {
                                        goto start;
                                    }
                                    break;


                                }
                        }

                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                            goto start;

                        break;
                    }
            }

        }

        static Car SearchCar(ref Project project)
        {
            Console.WriteLine("Список машин на проекте");
            foreach (Car projectCar in project.Cars)
            {
                if (projectCar.Active)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"Наименование : {projectCar.Name} Модель : {projectCar.Model} Гаражный номер : {projectCar.GarageNuber}");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Введите модель или гаражный номер машины");
            string modelORnumber = Console.ReadLine();
            return project.Cars.FirstOrDefault(c => c.Model == modelORnumber || c.GarageNuber == modelORnumber);
        }

        static Car SearchCarForStop(ref Project project)
        {
            Console.WriteLine("Список машин на проекте");
            foreach (Car projectCar in project.Cars)
            {
                if (projectCar.Active)
                {
                    Console.ForegroundColor = ConsoleColor.Green;


                    Console.WriteLine(
                        $"Наименование : {projectCar.Name} Модель : {projectCar.Model} Гаражный номер : {projectCar.GarageNuber}");
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Введите модель или гаражный номер машины");
            string modelORnumber = Console.ReadLine();
            return project.Cars.FirstOrDefault(c => c.Model == modelORnumber || c.GarageNuber == modelORnumber);
        }
        static Car SearchCar(ref CreateProject createProject)
        {
            Car mashina = new Car();
            int i = 0;
            Console.WriteLine("Все проекты");
            foreach (Project item in createProject.Projects)
            {
                Console.WriteLine($"{++i} {item.Name}");
            }
            proj:
            Console.WriteLine("Введите название проекта");
            string name = Console.ReadLine();
            Project project = createProject.SearchProject(ref createProject.Projects, name);


            if (project.Name != null)
            {
                Console.WriteLine("Список машин на проекте");
                foreach (Car projectCar in project.Cars)
                {
                    if (projectCar.Active)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine($"Наименование : {projectCar.Name} Модель : {projectCar.Model} Гаражный номер : {projectCar.GarageNuber}");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Введите гаражный номер или название модели машины");
                string garageNumber = Console.ReadLine();

                mashina = createProject.SerchCarByNumber(out string stroka, ref project, garageNumber);
                Console.WriteLine(stroka);


            }
            else goto proj;

            return mashina;
        }
        static Car SearchCar(ref CreateProject createProject, out Project pr)
        {
            Car mashina = new Car();
            int i = 0;
            Console.WriteLine("Все проекты");
            foreach (Project item in createProject.Projects)
            {
                Console.WriteLine($"{++i} {item.Name}");
            }
            proj:
            Console.WriteLine("Введите название проекта");
            string name = Console.ReadLine();
            Project project = createProject.SearchProject(ref createProject.Projects, name);


            if (project.Name != null)
            {
                Console.WriteLine("Список машин на проекте");
                foreach (Car projectCar in project.Cars)
                {
                    if (projectCar.Active)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine($"Наименование : {projectCar.Name} Модель : {projectCar.Model} Гаражный номер : {projectCar.GarageNuber}");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Введите гаражный номер или название модели машины");
                string garageNumber = Console.ReadLine();

                mashina = createProject.SerchCarByNumber(out string stroka, ref project, garageNumber);
                Console.WriteLine(stroka);


            }
            else goto proj;

            pr = project;
            return mashina;

        }

        public static bool LoginAdmin(CreateProject createProject)
        {

            int countOfIncorrectChance = 3;
            AdminMenu:
            Console.WriteLine("Введите логин");
            string login = Console.ReadLine();
            Console.WriteLine("Введите пароль");
            string password = Console.ReadLine();

            if (createProject.Admins != null)
            {
                Admin findElement = createProject.Admins.FirstOrDefault(w => w.Login == login && w.Password == password);
                if (findElement != null)
                {
                    Console.WriteLine("Добро пожаловать!");
                    Console.Clear();
                    return true;
                }
                else if (countOfIncorrectChance > 0)
                {
                    Console.WriteLine("Ваши данные некорректны!");
                    countOfIncorrectChance--;
                    Console.WriteLine("Осталось попыток : " + countOfIncorrectChance);
                    Thread.Sleep(30);
                    Console.Clear();
                    goto AdminMenu;
                }
            }

            return false;
        }
    }
}
