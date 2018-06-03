using System;

namespace Test.Classes
{
    [Serializable]
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
