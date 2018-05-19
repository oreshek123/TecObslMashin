using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TehObsluzMashin.DAL.Classes
{
    public class Part
    {
        public string Name { get; set; }
        public int ComponentId { get; set; }
        
        public void PrintComponents()
        { 
            Console.WriteLine($"Наименование : {Name}\tКод Компонента : {ComponentId}\n");
        }
    }
}
