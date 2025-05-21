using CardiologistAppointmentManager.Repositories;
using CardiologistAppointmentManager.Services;

namespace CardiologistAppointmentManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var _appointmentRepository = new AppointmentRepository();
            var appointmentService = new AppointmentService(_appointmentRepository);
            ManageAppointments manageAppointments = new ManageAppointments(appointmentService);
            manageAppointments.Start();
        }
    }
}
