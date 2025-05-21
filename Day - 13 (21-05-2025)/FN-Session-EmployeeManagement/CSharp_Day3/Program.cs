using CSharp_Day3.Models;
using CSharp_Day3.Repositories;
using CSharp_Day3.Services;

namespace CSharp_Day3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var employeeRepository = new EmployeeRepository();
            var employeeService = new EmployeeService(employeeRepository);
            ManageEmployee manageEmployee = new ManageEmployee(employeeService);
            manageEmployee.Start();
        }
    }
}
