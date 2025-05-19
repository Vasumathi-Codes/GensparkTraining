using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTasks
{
    internal class Task_01
    {
        public void Run() {
            Console.Write("Enter your name: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Please enter a Valid name");
            }
            else
            {
                Console.WriteLine($"Hello, {name.Trim()}!");
            }
        }
    }
}
