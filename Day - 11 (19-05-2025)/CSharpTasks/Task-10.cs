using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace CSharpTasks
{
    internal class Task_10
    {
        //10) write a program that accepts a 9-element array representing a Sudoku row.
        //Validates if the row:
        //Has all numbers from 1 to 9.
        //Has no duplicates.
        //Displays if the row is valid or invalid.

        public void Run()
        {
            int size = 9;
            Console.WriteLine("Enter a row for sudoku (9 numbers)");

            int[] sudokuRow = new int[size];
            bool[] numbers = new bool[size];

            Console.WriteLine("Enter the numbers:");
            for (int i = 0; i < size; i++)
            {
                Console.Write($"Element {i + 1}: ");
                while (!int.TryParse(Console.ReadLine(), out sudokuRow[i]) || sudokuRow[i] < 1 || sudokuRow[i] > 9)
                {
                    Console.Write($"Invalid input. Element {i + 1}: ");
                }
            }

            for(int i=0; i<size; i++)
            {
                if (numbers[sudokuRow[i]-1])
                {
                    Console.WriteLine("Invalid Sudoku Row !");
                    return;
                }
                numbers[sudokuRow[i] - 1] = true;
            }

            Console.WriteLine("The Sudoku row is valid");
        }
             

    }
}
