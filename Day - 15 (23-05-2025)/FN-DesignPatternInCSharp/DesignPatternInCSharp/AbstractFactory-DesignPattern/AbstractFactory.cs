using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternInCSharp.AbstractFactory_DesignPattern
{
    public static class AbstractFactory
    {
        public static void Run()
        {
            Console.WriteLine("Choose file type:");
            Console.WriteLine("1 - Text File");
            Console.WriteLine("2 - JSON File");
            Console.Write("Enter choice: ");

            IFileHandlerFactory factory = Console.ReadLine() == "2"
                ? new JsonFileFactory()
                : new TextFileFactory();

            IFileHandler handler = factory.CreateHandler("C:\\Users\\VC\\source\\repos\\DesignPatternInCSharp\\DesignPatternInCSharp\\AbstractFactory-DesignPattern\\abstractfile.txt");

            while (true)
            {
                Console.WriteLine("\nChoose action: 1 - Write, 2 - Read, 3 - Exit");
                string choice = Console.ReadLine() ?? "";

                if (choice == "1")
                {
                    Console.Write("Enter content to write: ");
                    string text = Console.ReadLine() ?? "";
                    handler.Write(text);
                }
                else if (choice == "2")
                {
                    Console.WriteLine("File Content:\n" + handler.Read());
                }
                else if (choice == "3")
                {
                    handler.Close();
                    Console.WriteLine("File closed. Exiting demo.");
                    break;
                }
            }
        }
    }

}
