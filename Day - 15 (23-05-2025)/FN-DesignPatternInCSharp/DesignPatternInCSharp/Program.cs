using System;
using DesignPatternInCSharp.AbstractFactory_DesignPattern;
using DesignPatternInCSharp.FactoryMethod_DesignPattern;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nSelect design pattern demo:");
            Console.WriteLine("1 - Singleton (FileHandler)");
            Console.WriteLine("2 - Factory Method");
            Console.WriteLine("3 - Abstract Factory");
            Console.WriteLine("0 - Exit");
            Console.Write("Enter choice: ");

            string? input = Console.ReadLine();
            if (input == "0") break;

            switch (input)
            {
                case "1":
                    Singleton.Run();
                    break;
                case "2":
                    FactoryMethod.Run();
                    break;
                case "3":
                    AbstractFactory.Run();
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
    }
}
