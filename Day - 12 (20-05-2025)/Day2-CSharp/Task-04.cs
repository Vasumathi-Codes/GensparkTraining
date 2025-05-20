using System;
using System.Collections.Generic;
using System.Linq;

class EmployeeApp_Hard
{
    static Dictionary<int, Employee> empDict = new Dictionary<int, Employee>();

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\n========= Employee Management System =========");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. List all Employees");
            Console.WriteLine("3. Find Employee by ID");
            Console.WriteLine("4. Update Employee Details");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Exit");
            Console.WriteLine("==============================================");

            int choice = Employee.ReadPositiveInt("Enter your choice: ");

            switch (choice)
            {
                case 1:
                    AddEmployee();
                    break;
                case 2:
                    ListAllEmployees();
                    break;
                case 3:
                    FindEmployeeById();
                    break;
                case 4:
                    UpdateEmployeeById();
                    break;
                case 5:
                    DeleteEmployeeById();
                    break;
                case 6:
                    Console.WriteLine("Exiting application. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
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
            Console.WriteLine("\nEmployee added successfully.");
        }
        else
        {
            Console.WriteLine("\nEmployee ID already exists.");
        }
    }

    static void FindEmployeeById()
    {
        int id = Employee.ReadNonNegativeInt("\nEnter employee ID to search: ");

        if (empDict.TryGetValue(id, out Employee? emp))
        {
            Console.WriteLine("\nEmployee Found:\n");
            Console.WriteLine(emp);
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    static void ListAllEmployees()
    {
        if (empDict.Count == 0)
        {
            Console.WriteLine("\n No employees found.");
            return;
        }

        Console.WriteLine("\n========= List of Employees =========");

        var sorted = empDict.Values
                            .OrderBy(e => e.Id);

        foreach (var emp in sorted)
        {
            Console.WriteLine(emp);
            Console.WriteLine("------------------------------------");
        }
    }

    static void UpdateEmployeeById()
    {
        int id = Employee.ReadNonNegativeInt("\nEnter the employee ID to update: ");

        if (empDict.TryGetValue(id, out Employee? emp))
        {
            Console.WriteLine("\nCurrent Employee Details:\n");
            Console.WriteLine(emp);

            Console.WriteLine("\nEnter new details:");
            emp.Name = Employee.ReadNonEmptyString("Updated Name    : ");
            emp.Age = Employee.ReadNonNegativeInt("Updated Age     : ");
            emp.Salary = Employee.ReadNonNegativeDouble("Updated Salary  : ");
            Console.WriteLine("\nEmployee updated successfully.");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    static void DeleteEmployeeById()
    {
        int id = Employee.ReadNonNegativeInt("\nEnter the employee ID to delete: ");

        if (empDict.Remove(id))
        {
            Console.WriteLine("\nEmployee deleted successfully.");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }
}
