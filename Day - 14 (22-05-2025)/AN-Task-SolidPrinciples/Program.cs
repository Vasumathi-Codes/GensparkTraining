using System;
using System.Text.RegularExpressions;
using System.Linq;
using AN_Task_SolidPrinciples.Single_Responsibility_Principle;
using AN_Task_SolidPrinciples.Open_closed_Principle;
using AN_Task_SolidPrinciples.Interface_Segregation_Principle;
using AN_Task_SolidPrinciples.Dependency_Inversion_Principle;

namespace AN_Task_SolidPrinciples
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nSOLID Principle Examples:");
                Console.WriteLine("1. SRP (Single Responsibility Principle)");
                Console.WriteLine("2. OCP (Open/Closed Principle)");
                Console.WriteLine("3. LSP (Liskov Substitution Principle)");
                Console.WriteLine("4. ISP (Interface Segregation Principle)");
                Console.WriteLine("5. DIP (Dependency Inversion Principle)");
                Console.WriteLine("Type 'exit' or 'q' to quit.");

                Console.Write("\nChoose a principle (1-5): ");
                string mainChoice = Console.ReadLine()?.Trim().ToLower();

                if (mainChoice == "exit" || mainChoice == "q")
                {
                    Console.WriteLine("Exiting program...");
                    break;
                }

                switch (mainChoice)
                {
                    case "1":
                        Console.WriteLine("\n1.1 SRP - Bad Example");
                        Console.WriteLine("1.2 SRP - Good Example");
                        Console.Write("\nChoose SRP version: ");
                        string srpChoice = Console.ReadLine();

                        Console.Write("\nEnter username: ");
                        string username = Console.ReadLine();
                        Console.Write("Enter password: ");
                        string password = Console.ReadLine();
                        Console.Write("Enter email: ");
                        string email = Console.ReadLine();

                        switch (srpChoice)
                        {
                            case "1.1":
                                var badService = new SRP_BadExample.UserService();
                                badService.Register(username, password, email);
                                break;

                            case "1.2":
                                var goodService = new UserService(
                                    new EmailValidator(),
                                    new PasswordValidator(),
                                    new UserRepository(),
                                    new EmailSender(),
                                    new Logger()
                                );
                                goodService.Register(username, password, email);
                                break;

                            default:
                                Console.WriteLine("Invalid SRP choice.");
                                break;
                        }
                        break;

                    case "2":
                        Console.WriteLine("\n2.1 OCP - Bad Example");
                        Console.WriteLine("2.2 OCP - Good Example");
                        Console.Write("\nChoose OCP version: ");
                        string ocpChoice = Console.ReadLine();

                        Console.Write("Enter membership type (Basic/Silver/Gold): ");
                        string membershipType = Console.ReadLine();

                        switch (ocpChoice)
                        {
                            case "2.1":
                                var badCalculator = new OCP_BadExample.MembershipFeeCalculator();
                                double badFee = badCalculator.CalculateFee(membershipType);
                                Console.WriteLine($"[Bad] Fee for {membershipType}: ${badFee}");
                                break;

                            case "2.2":
                                OCP_GoodPractice.IMembership membership = membershipType switch
                                {
                                    "Basic" => new OCP_GoodPractice.BasicMembership(),
                                    "Silver" => new OCP_GoodPractice.SilverMembership(),
                                    "Gold" => new OCP_GoodPractice.GoldMembership(),
                                    _ => null
                                };

                                if (membership == null)
                                {
                                    Console.WriteLine("Invalid membership type.");
                                    break;
                                }

                                var goodCalculator = new OCP_GoodPractice.MembershipFeeCalculator();
                                double goodFee = goodCalculator.CalculateFee(membership);
                                Console.WriteLine($"[Good] Fee for {membershipType}: ${goodFee}");
                                break;

                            default:
                                Console.WriteLine("Invalid OCP choice.");
                                break;
                        }
                        break;

                    case "3":
                        Console.WriteLine("\n3.1 LSP - Bad Example");
                        Console.WriteLine("3.2 LSP - Good Example");
                        Console.Write("\nChoose LSP version: ");
                        string lspChoice = Console.ReadLine();

                        switch (lspChoice)
                        {
                            case "3.1":
                                try
                                {
                                    Liskov_substitution_Principle.LSP_BadExample.Test();
                                }
                                catch (NotSupportedException ex)
                                {
                                    Console.WriteLine($"Exception caught: {ex.Message}");
                                }
                                break;

                            case "3.2":
                                Liskov_substitution_Principle.LSP_GoodPractice.Test();
                                break;

                            default:
                                Console.WriteLine("Invalid LSP choice.");
                                break;
                        }
                        break;

                    case "4":
                        Console.WriteLine("\n4.1 ISP - Bad Example");
                        Console.WriteLine("4.2 ISP - Good Example");
                        Console.Write("\nChoose ISP version: ");
                        string ispChoice = Console.ReadLine();

                        switch (ispChoice)
                        {
                            case "4.1":
                                try
                                {
                                    var badPrinter = new ISP_BadExample.SimplePrinter();
                                    badPrinter.Print("ISP Bad Document");
                                    badPrinter.Scan("ISP Bad Document");  // Will throw NotImplementedException
                                }
                                catch (NotImplementedException ex)
                                {
                                    Console.WriteLine($"Exception caught: {ex.Message}");
                                }
                                break;

                            case "4.2":
                                var oldPrinter = new ISP_GoodPractice.OldPrinter();
                                oldPrinter.Print("OldPrinter Document");

                                var mfp = new ISP_GoodPractice.MultiFunctionPrinter();
                                mfp.Print("MultiFunctionPrinter Document");
                                mfp.Scan("MultiFunctionPrinter Document");
                                mfp.Fax("MultiFunctionPrinter Document");
                                break;

                            default:
                                Console.WriteLine("Invalid ISP choice.");
                                break;
                        }
                        break;

                    case "5":
                        Console.WriteLine("\n5.1 DIP - Bad Example");
                        Console.WriteLine("5.2 DIP - Good Example");
                        Console.Write("\nChoose DIP version: ");
                        string dipChoice = Console.ReadLine();

                        switch (dipChoice)
                        {
                            case "5.1":
                                DIP_BadExample.Run();
                                break;

                            case "5.2":
                                DIP_GoodPractice.Run();
                                break;

                            default:
                                Console.WriteLine("Invalid DIP choice.");
                                break;
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
