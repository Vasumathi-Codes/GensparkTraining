using System.Threading.Tasks;

namespace CSharpTasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Choose a task to run:");
                Console.WriteLine("1  - Greet the user by name");
                Console.WriteLine("2  - Find the largest of two numbers");
                Console.WriteLine("3  - Perform an arithmetic operation on two numbers");
                Console.WriteLine("4  - Login system with 3 attempts (username: Admin, password: pass)");
                Console.WriteLine("5  - Count numbers divisible by 7 (input 10 numbers)");
                Console.WriteLine("6  - Frequency count of elements in an array");
                Console.WriteLine("7  - Left rotate array by one position");
                Console.WriteLine("8  - Merge two integer arrays");
                Console.WriteLine("9  - Bulls and Cows word game (4-letter guess vs secret word)");
                Console.WriteLine("10 - Validate a Sudoku row (9 unique numbers from 1 to 9)");
                Console.WriteLine("11 - Validate entire Sudoku board (9x9 grid)");
                Console.WriteLine("12 - Encrypt/Decrypt message using Caesar cipher (shift by 3)");
                Console.WriteLine("0  - Exit");
                Console.Write("Enter your choice: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        var t1 = new Task_01();
                        t1.Run();
                        break;

                    case "2":
                        var t2 = new Task_02();
                        t2.Run(); 
                        break;

                    case "3":
                        var t3 = new Task_03();
                        t3.Run();
                        break;

                    case "4":
                        var t4 = new Task_04();
                        t4.Run();
                        break;

                    case "5":
                        var t5 = new Task_05();
                        t5.Run();
                        break;

                    case "6":
                        var t6 = new Task_06();
                        t6.Run();
                        break;

                    case "7":
                        var t7 = new Task_07();
                        t7.Run();
                        break;

                    case "8":
                        var t8 = new Task_08();
                        t8.Run();
                        break;

                    case "9":
                        var t9 = new Task_09();
                        t9.Run();
                        break;

                    case "10":
                        var t10 = new Task_10();
                        t10.Run();
                        break;

                    case "11":
                        var t11 = new Task_11();
                        t11.Run();
                        break;

                    case "12":
                        var t12 = new Task_12();
                        t12.Run();
                        break;

                    case "0":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
