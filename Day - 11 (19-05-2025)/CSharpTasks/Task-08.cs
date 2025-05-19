using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTasks
{
    internal class Task_08
    {
        // 8) Given two integer arrays, merge them into a single array.
        // Input: {1, 3, 5} and {2, 4, 6}
        // Output: {1, 3, 5, 2, 4, 6}
        public void Run()
        {
            Console.Write("Enter the size of first array ");
            int size1;
            while (!int.TryParse(Console.ReadLine(), out size1) || size1 <= 0)
            {
                Console.Write("Invalid input. Please enter a positive integer: ");
            }

            int[] numbers1 = new int[size1];
            Console.WriteLine("Enter the numbers for first array :");
            for (int i = 0; i < size1; i++)
            {
                Console.Write($"Element {i + 1}: ");
                while (!int.TryParse(Console.ReadLine(), out numbers1[i]))
                {
                    Console.Write($"Invalid input. Element {i + 1}: ");
                }
            }

            Console.Write("Enter the size of second array: ");
            int size2;
            while (!int.TryParse(Console.ReadLine(), out size2) || size2 <= 0)
            {
                Console.Write("Invalid input. Please enter a positive integer: ");
            }

            int[] numbers2 = new int[size2];
            Console.WriteLine("Enter the numbers for 2nd array:");
            for (int i = 0; i < size2; i++)
            {
                Console.Write($"Element {i + 1}: ");
                while (!int.TryParse(Console.ReadLine(), out numbers2[i]))
                {
                    Console.Write($"Invalid input. Element {i + 1}: ");
                }
            }

            int[] result = new int[size1 + size2];
            for (int i = 0; i < size1; i++)
            {
                result[i] = numbers1[i];
            }

            for (int j = 0; j < size2; j++)
            {
                result[j + size1] = numbers2[j];
            }

            Console.WriteLine("Result Array after merging");
            for (int i = 0; i < size1 + size2; i++)
                Console.Write($"{result[i]} ");

            Console.WriteLine();
        }
    }
}
