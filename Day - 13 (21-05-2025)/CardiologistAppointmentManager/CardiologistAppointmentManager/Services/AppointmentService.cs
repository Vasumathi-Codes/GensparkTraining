using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardiologistAppointmentManager.Interfaces;
using CardiologistAppointmentManager.Models;
using CardiologistAppointmentManager.Repositories;

namespace CardiologistAppointmentManager.Services
{
    public class AppointmentService : IAppointmentService
    {
        IRepository<int, Appointment> _appointmentRepository;

        public AppointmentService(IRepository<int, Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public int AddPatient(Appointment appointment)
        {
            try
            {
                Appointment newAppointment = _appointmentRepository.Add(appointment);
                if (newAppointment != null)
                {
                    return appointment.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }

        public ICollection<Appointment> SearchAppointments(SearchModel searchModel)
        {
            try
            {
                var appointments = _appointmentRepository.GetAll();
                appointments = SearchByName(appointments, searchModel.PatientName);
                appointments = SearchByAppointmentDate(appointments, searchModel.AppointmentDate);
                appointments = SearchByAge(appointments, searchModel.AgeRange);
                if (appointments != null || appointments.Count > 0)
                {
                    return appointments.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private ICollection<Appointment> SearchByAge(ICollection<Appointment> appointments, Range<int>? ageRange)
        {
            if(ageRange == null || appointments == null)
            {
                return appointments;
            }
            return appointments.Where(a=> a.PatientAge >= ageRange.MinVal && a.PatientAge <= ageRange.MaxVal).ToList();
        }

        private ICollection<Appointment> SearchByAppointmentDate(ICollection<Appointment> appointments, DateOnly? appointmentDate)
        {
            if(appointmentDate == null || appointments.Count == 0)
            {
                return appointments;
            }
            return appointments
                .Where(a => a.AppointmentDate == appointmentDate.Value)
                .ToList();

        }

        private ICollection<Appointment> SearchByName(ICollection<Appointment> appointments, string patientName)
        {
            if(patientName == null || appointments == null)
            {
                return appointments;
            }
            return appointments
                .Where(a => !string.IsNullOrEmpty(a.PatientName) && a.PatientName.Contains(patientName, StringComparison.OrdinalIgnoreCase))
                .ToList();

        }
    }
}
