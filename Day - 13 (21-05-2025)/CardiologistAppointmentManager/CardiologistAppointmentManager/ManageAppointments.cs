using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardiologistAppointmentManager.Interfaces;
using CardiologistAppointmentManager.Models;

namespace CardiologistAppointmentManager
{
    public class ManageAppointments
    {
        private readonly IAppointmentService _appointmentService;
        public ManageAppointments(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public void Start()
        {

            while (true)
            {
                Console.WriteLine("\n=== Appointment Management System ===");
                Console.WriteLine("1. Add Patient");
                Console.WriteLine("2. Search Patient");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Appointment appointment = new Appointment();
                        appointment.TakeAppointmentDetailsFromUser();
                        int result = _appointmentService.AddPatient(appointment);
                        Console.WriteLine((result != -1 ) ? $"Appointment is added with id : {appointment.Id}": "Error Creating appointment. Try again !");
                        break;

                    case "2":
                        SearchModel search = new SearchModel();

                        Console.Write("Enter Name (leave blank to skip): ");
                        string? name = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(name)) search.PatientName = name;

                        Console.Write("Enter Min Age (leave blank to skip): ");
                        string? minAgeStr = Console.ReadLine();
                        Console.Write("Enter Max Age (leave blank to skip): ");
                        string? maxAgeStr = Console.ReadLine();
                        if (int.TryParse(minAgeStr, out int minAge) && int.TryParse(maxAgeStr, out int maxAge))
                            search.AgeRange = new Range<int> { MinVal = minAge, MaxVal = maxAge };

                        Console.Write("Enter Appointment Date (yyyy-MM-dd) (leave blank to skip): ");
                        string? dateInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(dateInput))
                        {
                            if (DateOnly.TryParse(dateInput, out DateOnly appointmentDate))
                            {
                                search.AppointmentDate = appointmentDate;
                            }
                            else
                            {
                                Console.WriteLine("Invalid date format. Skipping date filter.");
                            }
                        }

                        var results = _appointmentService.SearchAppointments(search);
                        if (results != null && results.Count > 0)
                        {
                            Console.WriteLine("Appointment matching the Search criteria:");
                            foreach (var e in results)
                            {
                                Console.WriteLine();
                                Console.WriteLine(e);
                                Console.WriteLine("----------------------------------");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Patient found matching the criteria.");
                        }
                        break;


                    case "4":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}
