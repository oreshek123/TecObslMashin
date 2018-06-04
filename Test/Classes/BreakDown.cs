using System;

namespace Test.Classes
{
    [Serializable]
    public class BreakDown
    {
        public DateTime DateOfIssue { get; set; }
        public Car Car { get; set; }
        public string DescriptionOfBreakDown { get; set; }
        public string RecommendationsForRepeiring { get; set; }
        public User User { get; set; }
        public Project Project { get; set; }

        public void PrintBreakDown()
        {
            Car?.PrintInfo();
            Console.WriteLine($"Дата создания : {DateOfIssue}\n" +
                              $"Описание поломки : {DescriptionOfBreakDown}\n" +
                              $"Рекомендации по починке : {RecommendationsForRepeiring}\n" +
                              $"Проект : {Project.Name}\n" +
                              $"Пользователь, который проводил осмотр : {User.Login}\n");  
        }
    }
}
