using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CardiologistAppointmentManager.Helpers;

namespace CardiologistAppointmentManager.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;

        public Appointment() { }

        public void TakeAppointmentDetailsFromUser()
        {
            PatientName = InputHelper.ReadNonEmptyString("Please enter the patient's name: ", "Patient's Name is required.");

            PatientAge = InputHelper.ReadPositiveInt("Please enter the patient's age: ", "Age should be greater than 0:");

            AppointmentDate = InputHelper.ReadFutureDate("Please enter the appointment date (yyyy-MM-dd): ", "Invalid date.");

            Console.Write("Please enter the reason for the appointment: ");
            Reason = Console.ReadLine() ?? "";
        }

        public override string ToString()
        {
            return "Patient ID : " + Id + "\nPatient Name : " + PatientName + "\nPatient Age : " + PatientAge + "\nAppointment Date : " + AppointmentDate + "\nReason : " + Reason;
        }

    }
}
