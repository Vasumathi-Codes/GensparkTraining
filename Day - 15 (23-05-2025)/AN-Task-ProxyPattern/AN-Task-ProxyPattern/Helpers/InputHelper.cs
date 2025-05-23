using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AN_Task_ProxyPattern.Helpers
{
    public class InputHelper
    {
        public static int ReadPositiveInt(string prompt, string errorMsg = "Invalid input. Please enter a positive integer.")
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out result) && result > 0)
                    return result;
                Console.WriteLine(errorMsg);
            }
        }

        public static int ReadNonNegativeInt(string prompt, string errorMsg = "Invalid input. Please enter a non-negative integer.")
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out result) && result >= 0)
                    return result;
                Console.WriteLine(errorMsg);
            }
        }

        public static string ReadNonEmptyString(string prompt, string errorMsg = "Input cannot be empty.")
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;
                Console.WriteLine(errorMsg);
            }
        }

        public static DateOnly ReadFutureDate(string prompt, string errorMsg = "Invalid input. Please enter a valid future date (yyyy-MM-dd).")
        {
            DateOnly date;
            while (true)
            {
                Console.Write(prompt);
                if (DateOnly.TryParse(Console.ReadLine(), out date) && date >= DateOnly.FromDateTime(DateTime.Today))
                    return date;
                Console.WriteLine(errorMsg);
            }
        }


    }
}
