using System;
using Test.Classes;

namespace ModuleBreakDown
{
    
    [Serializable]
    public class CreateBreakDown
    {  
        public BreakDown Create(ref Car car, ref Project project)
        {
            Console.WriteLine("Введите описание поломки");
            string description = Console.ReadLine();
            Console.WriteLine("Введите рекомендации по починке");
            string recommends = Console.ReadLine();
            user:
            Console.WriteLine("Введите логин пользователя, который проводил осмотр ");
            string login = Console.ReadLine();
            User user = new User();
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
            BreakDown breakDown = new BreakDown()
            {
                Car = car,
                DateOfIssue = DateTime.Now,
                DescriptionOfBreakDown = description,
                RecommendationsForRepeiring = recommends,
                User = user,
                Project = project
            };

            return breakDown;
        }
        public BreakDown Create(ref Car car, ref Project project, ref User user)
        {
            Console.WriteLine("Введите описание поломки");
            string description = Console.ReadLine();
            Console.WriteLine("Введите рекомендации по починке");
            string recommends = Console.ReadLine();

            BreakDown breakDown = new BreakDown()
            {
                Car = car,
                DateOfIssue = DateTime.Now,
                DescriptionOfBreakDown = description,
                RecommendationsForRepeiring = recommends,
                User = user,
                Project = project
            };

            return breakDown;
        }
    }
    
}
