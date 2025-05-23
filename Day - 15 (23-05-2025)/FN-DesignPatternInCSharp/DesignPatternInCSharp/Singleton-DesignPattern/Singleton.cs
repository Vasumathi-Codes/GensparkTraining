using System;
using System.Drawing;
using DesignPatternInCSharp.Singleton_DesignPattern;

public static class Singleton
{
    public static void Run()
    {
        // Singleton ensures that only one instance of a class exists throughout the application.
        // It provides a global point of access to that single instance.
        var fileHandler = FileHandlerSingleton.GetInstance("C:\\Users\\VC\\source\\repos\\DesignPatternInCSharp\\DesignPatternInCSharp\\Singleton-DesignPattern\\signletonfile.txt");

        while (true)
        {
            Console.WriteLine("\nSingleton Demo - Choose action:");
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
                Console.WriteLine("File closed and exiting Singleton demo.");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice, try again.");
            }
        }
    }
}
