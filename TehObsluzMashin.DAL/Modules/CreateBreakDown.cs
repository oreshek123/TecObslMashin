using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TehObsluzMashin.DAL.Classes;
namespace TehObsluzMashin.DAL.Modules
{
    public class CreateBreakDown
    {
        public BreakDown Create(ref Car car, string description, string recommends,ref User user)
        {
            BreakDown breakDown = new BreakDown()
            {
                Car = car,
                DateOfIssue = DateTime.Now,
                DescriptionOfBreakDown = description,
                RecommendationsForRepeiring = recommends,
                User = user
            };

            return breakDown;
        }
    }
}
