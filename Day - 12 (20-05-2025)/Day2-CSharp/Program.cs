namespace Day2_CSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Select the task to run:");
                Console.WriteLine("1 - Jagged array for Posts");
                Console.WriteLine("2 - Employee Promotion with List<String>");
                Console.WriteLine("3 - EmployeeApp - Medium");
                Console.WriteLine("4 - EmployeeApp - Hard");
                Console.WriteLine("0 - Exit");
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();

                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        var t1 = new Task_01();
                        t1.Run();
                        break;
                    case 2:
                        var t2 = new EmployeePromotion();
                        t2.Run();
                        break;
                    case 3:
                        var empPromotion = new EmployeeApp();
                        empPromotion.Run();
                        break;

                    case 4:
                        var empPromotionHard = new EmployeeApp_Hard();
                        empPromotionHard.Run();
                        break;
                    case 0:
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid task.");
                        break;
                }

                Console.WriteLine(); 
            }
        }
    }
}
