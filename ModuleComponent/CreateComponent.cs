using System;
using System.Collections.Generic;
using System.Threading;
using RandomNameGenerator;
using Test.Classes;

namespace ModuleComponent
{
    public class CreateComponent
    {
        private Random Rnd = new Random();
        public List<Part> GenerateComponents()
        {
            List<Part> Components = new List<Part>();

            for (int i = 0; i < Rnd.Next(1, 20); i++)
            {
                Part component = new Part()
                {
                    ComponentId = Rnd.Next(1000, 10000),
                    Name = NameGenerator.GenerateLastName().ToLower()
                };
                Thread.Sleep(30);
                Components.Add(component);
            }

            return Components;
        }

        public static Part CtComponent(int id)
        {
            
            Console.WriteLine("Введите наименование для компонента ");
            string name = Console.ReadLine();
            Part component = new Part()
            {
                ComponentId = id,
                Name = name
            };
            return component;
        }
    }
}
