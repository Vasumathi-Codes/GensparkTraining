﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardiologistAppointmentManager.Models;

namespace CardiologistAppointmentManager.Repositories
{
    public class AppointmentRepository : Repository<int, Appointment>
    {
        public AppointmentRepository():base() { }

        public override ICollection<Appointment> GetAll()
        {
            if(_items.Count == 0)
            {
                return null;
            }
            return _items;
        }

        public override Appointment GetById(int id)
        {
            var appointment = _items.FirstOrDefault(i => i.Id == id);
            if (appointment == null)
                throw new KeyNotFoundException("Patient not found");
            
            return appointment;
        }

        protected override int GenerateID()
        {
            if (_items.Count == 0)
            {
                return 1;
            }
            else
            {
                return _items.Max(i => i.Id) + 1;
            }
        }
    }
}
