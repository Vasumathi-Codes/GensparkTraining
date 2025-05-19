using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTasks
{
    internal class Task_05
    {
        // 5) Take 10 numbers from user and print the number of numbers that are divisible by 7
        public void Run()
        {
            int count = 0;
            int totalNumbers = 10;

            Console.WriteLine($"Enter {totalNumbers} numbers:");

            for (int i = 1; i <= totalNumbers; i++)
            {
                int num;
                Console.Write($"Enter number {i}: ");
                while (!int.TryParse(Console.ReadLine(), out num))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer:");
                    Console.Write($"Enter number {i}: ");
                }
                if (num % 7 == 0)
                {
                    count++;
                }
            }

            Console.WriteLine($"\nOut of {totalNumbers} numbers, {count} are divisible by 7.");
        }
    }
}
