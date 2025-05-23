using System;

namespace DesignPatternInCSharp.FactoryMethod_DesignPattern
{
    public static class FactoryMethod
    {
        public static void Run()
        {
            Console.WriteLine("\n--- Factory Method Demo ---");
            var fileHandler = FileHandlerFactory.CreateFileHandler("C:\\Users\\VC\\source\\repos\\DesignPatternInCSharp\\DesignPatternInCSharp\\FactoryMethod-DesignPattern\\factoryfile.txt");

            while (true)
            {
                Console.WriteLine("\nFactory Method Demo - Choose action:");
                Console.WriteLine("1 - Write to file");
                Console.WriteLine("2 - Read file content");
                Console.WriteLine("3 - Close file and exit demo");
                Console.Write("Enter choice: ");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Enter text to write: ");
                    var text = Console.ReadLine() ?? "";
                    fileHandler.Write(text + Environment.NewLine);
                    Console.WriteLine("Written to file.");
                }
                else if (choice == "2")
                {
                    Console.WriteLine("File content:");
                    Console.WriteLine(fileHandler.ReadAll());
                }
                else if (choice == "3")
                {
                    fileHandler.Close();
                    Console.WriteLine("File closed and exiting Factory Method demo.");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice, try again.");
                }
            }
        }
    }
}
