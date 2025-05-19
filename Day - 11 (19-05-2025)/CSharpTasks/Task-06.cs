using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTasks
{
    internal class Task_06
    {
        // 6) Count the Frequency of Each Element
        // Given an array, count the frequency of each element and print the result.
        // Input: {1, 2, 2, 3, 4, 4, 4}

        // output
        // 1 occurs 1 times  
        // 2 occurs 2 times  
        // 3 occurs 1 times  
        // 4 occurs 3 times
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

            Array.Sort(numbers);
            Console.WriteLine("\nFrequency of each element:");
            int count = 1;
            for (int i = 1; i < size; i++)
            {
                if (numbers[i] == numbers[i - 1])
                {
                    count++;
                }
                else
                {
                    Console.WriteLine($"{numbers[i - 1]} occurs {count} time(s)");
                    count = 1;
                }
            }
            Console.WriteLine($"{numbers[size - 1]} occurs {count} time(s)");
        }

    }
}
