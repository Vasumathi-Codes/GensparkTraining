using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTasks
{
    internal class Task_03
    {
        public void Run()
        {
            double num1, num2, result;

            Console.WriteLine("Enter 2 numbers :");
            while (!double.TryParse(Console.ReadLine(), out num1))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer:");
            }

            while (!double.TryParse(Console.ReadLine(), out num2))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer:");
            }

            Console.WriteLine("Enter operation to Perform");
            Console.WriteLine("Add +");
            Console.WriteLine("Subtract -");
            Console.WriteLine("Multiply *");
            Console.WriteLine("Divide /");
            string op = Console.ReadLine();
            switch (op)
            {
                case "+":
                    result = num1 + num2;
                    Console.WriteLine($"Result: {num1} + {num2} = {result}");
                    break;
                case "-":
                    result = num1 - num2;
                    Console.WriteLine($"Result: {num1} - {num2} = {result}");
                    break;
                case "*":
                    result = num1 * num2;
                    Console.WriteLine($"Result: {num1} * {num2} = {result}");
                    break;
                case "/":
                    if (num2 == 0)
                    {
                        Console.WriteLine("Division by zero is not allowed.");
                    }
                    else
                    {
                        result = num1 / num2;
                        Console.WriteLine($"Result: {num1} / {num2} = {result}");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid operation! Please enter one of +, -, *, /.");
                    break;
            }
        }
    }
}
