using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TehObsluzMashin.DAL.Classes
{
    public enum Type
    {
        Mehanic, Auto
    }
    public class Car
    {
        public string Model { get; set; }
        public int YearOfIssue { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public string GarageNuber { get; set; }
        public bool Active { get; set; } = true;

        public List<Part> Components { get; set; }

        public void PrintOnlyComponents()
        {
            foreach (Part component in Components)
            {
                component.PrintComponents();
            }
        }
        public void PrintInfo()
        {
            Console.WriteLine("--------------------Машина------------------");
            if (Active)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Модель : {Model}\t" +
                                  $"Год выпуска : {YearOfIssue}\t" +
                                  $"Наименование : {Name}\t" +
                                  $"Тип : {Type}\t" +
                                  $"Гаражный номер : {GarageNuber}\n");
                Console.ForegroundColor = ConsoleColor.White;
                if(Components!=null)
                {
                    foreach (Part component in Components)
                {
                    component.PrintComponents();
                }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Модель : {Model}\t" +
                                  $"Год выпуска : {YearOfIssue}\t" +
                                  $"Наименование : {Name}\t" +
                                  $"Тип : {Type}\t" +
                                  $"Гаражный номер : {GarageNuber}\n");
                Console.ForegroundColor = ConsoleColor.White;
                if(Components!=null)
                {
                    foreach (Part component in Components)
                {
                    component.PrintComponents();
                }
                }
            }
        } 
    }
}
