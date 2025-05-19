using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTasks
{
    internal class Task_02
    {
        public void Run()
        {
            int num1, num2;

            Console.WriteLine("Enter 2 numbers :");
            while (!int.TryParse(Console.ReadLine(), out num1))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer:");
            }

            while (!int.TryParse(Console.ReadLine(), out num2))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer:");
            }

            if (num1 > num2)
            {
                Console.WriteLine($"num1 : {num1} is the largest");
            }
            else if (num2 > num1)
            {
                Console.WriteLine($"num2 : {num2} is the largest");
            }
            else
            {
                Console.WriteLine($"Both are equal : {num1}");
            }
        }
    }
}
