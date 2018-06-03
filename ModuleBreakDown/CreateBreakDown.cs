using System;
using Test.Classes;

namespace ModuleBreakDown
{
    [Serializable]
    public class CreateBreakDown
    {
        public BreakDown Create(ref Car car, string description, string recommends,ref User user, ref Project project)
        {
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
