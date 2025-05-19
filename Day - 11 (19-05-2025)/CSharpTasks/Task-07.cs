using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTasks
{
    internal class Task_07
    {
        // 7) create a program to rotate the array to the left by one position.
        // Input: {10, 20, 30, 40, 50}
        // Output: {20, 30, 40, 50, 10}
        public void Run()
        {
            Console.Write("Enter the number of elements: ");
            int size;
            while (!int.TryParse(Console.ReadLine(), out size) || size <= 0)
            {
                Console.Write("Invalid input. Please enter a positive integer: ");
            }

            int[] numbers = new int[size];
            Console.WriteLine("Enter the numbers:");
            for (int i = 0; i < size; i++)
            {
                Console.Write($"Element {i + 1}: ");
                while (!int.TryParse(Console.ReadLine(), out numbers[i]))
                {
                    Console.Write($"Invalid input. Element {i + 1}: ");
                }
            }

            int firstNum = numbers[0];
            //[1, 2, 3, 4, 5]
            //[2, 3, 4, 5, 5]
            for (int i = 0; i < size - 1; i++)
            {
                numbers[i] = numbers[i + 1];
            }
            numbers[size - 1] = firstNum;

            Console.WriteLine("Array after rotating left by one place");
            for (int i = 0; i < size; i++)
                Console.Write($"{numbers[i]} ");

            Console.WriteLine();
        }
    }
}
