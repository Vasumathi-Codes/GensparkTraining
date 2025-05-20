using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class EmployeePromotion
{
    static List<string> promotionList = new List<string>();

    public void Run()
    {
        int choice;
        do
        {
            Console.WriteLine("\n==================================================================");
            Console.WriteLine("--- Employee Promotion Tasks ---");
            Console.WriteLine("1. Add employee names");
            Console.WriteLine("2. Find promotion position of an employee");
            Console.WriteLine("3. Trim extra memory used by the list");
            Console.WriteLine("4. Print promoted employee list in ascending order");
            Console.WriteLine("5. Exit");
            Console.WriteLine("==================================================================\n");

            Console.Write("Enter your choice (1-5): ");
            bool isValid = int.TryParse(Console.ReadLine(), out choice);
            if (!isValid)
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddEmployeeNames();
                    break;
                case 2:
                    FindPromotionPosition();
                    break;
                case 3:
                    TrimListMemory();
                    break;
                case 4:
                    PrintSortedEmployeeList();
                    break;
                case 5:
                    Console.WriteLine("Exiting program.");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }

        } while (choice != 5);
    }

    static void AddEmployeeNames()
    {
        Console.WriteLine("\nPlease enter the employee names in the order of their eligibility for promotion (Enter blank to stop):");
        while (true)
        {
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
                break;
            promotionList.Add(name);
        }

        Console.WriteLine("\nEmployees added:");
        foreach (var name in promotionList)
        {
            Console.WriteLine(name);
        }
    }

    static void FindPromotionPosition()
    {
        if (promotionList.Count == 0)
        {
            Console.WriteLine("\nNo employees in the promotion list.");
            return;
        }

        string name = Employee.ReadNonEmptyString("Please enter the name of the employee to check promotion position: ");

        int index = promotionList.FindIndex(e => e.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (index >= 0)
            Console.WriteLine($"\n{name} is at the position {index + 1} for promotion.");
        else
            Console.WriteLine($"\n{name} is not found in the promotion list.");
    }


    static void TrimListMemory()
    {
        promotionList.Add("Franky");
        promotionList.Add("Brook");
        promotionList.Add("Jimbe");
        Console.WriteLine($"\nThe current size (capacity) of the collection is: {promotionList.Capacity}");
        promotionList.TrimExcess();
        Console.WriteLine($"The size after removing the extra space is: {promotionList.Capacity}");
    }

    static void PrintSortedEmployeeList()
    {
        if (promotionList.Count == 0)
        {
            Console.WriteLine("\nNo employees in the promotion list.");
            return;
        }

        List<string> sortedList = new List<string>(promotionList);
        sortedList.Sort();

        Console.WriteLine("\nPromoted employee list in ascending order:");
        foreach (var name in sortedList)
        {
            Console.WriteLine(name);
        }
    }
}

