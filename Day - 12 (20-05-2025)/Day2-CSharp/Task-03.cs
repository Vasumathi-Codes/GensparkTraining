using System;
using System.Collections.Generic;
using System.Linq;

class EmployeeApp
{
    static Dictionary<int, Employee> empDict = new Dictionary<int, Employee>();

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\n==================================================================");
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. Display all employees sorted by salary");
            Console.WriteLine("3. Find employee by ID");
            Console.WriteLine("4. Find all employees with a given name");
            Console.WriteLine("5. Find employees elder than a given employee (by ID)");
            Console.WriteLine("6. Exit");
            Console.WriteLine("\n==================================================================\n");

            int choice = Employee.ReadPositiveInt("Enter you choice : ");

            switch (choice)
            {
                case 1:
                    AddEmployee();
                    break;
                case 2:
                    SortEmployeesBySalary();
                    break;
                case 3:
                    FindEmployeeById();
                    break;
                case 4:
                    FindEmployeesByName();
                    break;
                case 5:
                    FindEmployeesElderThan();
                    break;
                case 6:
                    return;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }

    static void AddEmployee()
    {
        Employee emp = new Employee();
        emp.TakeEmployeeDetailsFromUser();

        if (!empDict.ContainsKey(emp.Id))
        {
            empDict.Add(emp.Id, emp);
            Console.WriteLine("Employee added successfully.");
        }
        else
        {
            Console.WriteLine("Employee ID already exists.");
        }
    }

    static void SortEmployeesBySalary()
    {
        if (empDict.Count == 0)
        {
            Console.WriteLine("No employees to display.");
            return;
        }

        var sorted = empDict.Values.OrderByDescending(e => e.Salary).ToList();
        Console.WriteLine("\nEmployees sorted by salary:");
        foreach (var emp in sorted)
        {
            Console.WriteLine("\n-------------------------------------");
            Console.WriteLine(emp);
            Console.WriteLine("-------------------------------------");
        }
    }


    static void FindEmployeeById()
    {
        int id = Employee.ReadNonNegativeInt("\nEnter employee ID to search: ");

        var result = empDict.Values
                            .Where(e => e.Id == id)
                            .FirstOrDefault();

        if (result != null)
        {
            Console.WriteLine("\nEmployee Found:\n");
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    static void FindEmployeesByName()
    {
        string name = Employee.ReadNonEmptyString("Enter employee name to search: ");

        var matches = empDict.Values
            .Where(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matches.Count > 0)
        {
            Console.WriteLine("Matching employees:");
            foreach (var e in matches)
            {
                Console.WriteLine("\n----------------------------------");
                Console.WriteLine(e);
                Console.WriteLine("----------------------------------");
            }
        }
        else
        {
            Console.WriteLine("No employee found with that name.");
        }
    }

    static void FindEmployeesElderThan()
    {
        int id = Employee.ReadNonNegativeInt("Enter employee ID to compare age: ");

        var emp = empDict.ContainsKey(id) ? empDict[id] : null;

        if (emp != null)
        {
            var elders = empDict.Values.
                Where(e => e.Age > emp.Age)
                .ToList();
            Console.WriteLine($"Employees elder than {emp.Name}:");

            if (elders.Count == 0)
            {
                Console.WriteLine("No employees are elder.");
            }
            else
            {
                foreach (var e in elders)
                {
                    Console.WriteLine("\n-------------------------------------");
                    Console.WriteLine(e);
                    Console.WriteLine("--------------------------------------");
                }
            }
        }
        else
        {
            Console.WriteLine("Employee ID not found.");
        }
    }
}
