using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardiologistAppointmentManager.Models;

namespace CardiologistAppointmentManager.Interfaces
{
    public interface IAppointmentService
    {
        public int AddPatient(Appointment appointment);
        public ICollection<Appointment> SearchAppointments(SearchModel search);

    }
}
