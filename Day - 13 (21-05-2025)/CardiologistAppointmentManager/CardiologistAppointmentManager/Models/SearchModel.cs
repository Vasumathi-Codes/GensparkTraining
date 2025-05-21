using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CardiologistAppointmentManager.Models
{
    public class SearchModel
    {
        public string? PatientName { get; set; }
        public DateOnly? AppointmentDate { get; set; }
        public Range<int>? AgeRange { get; set; }
    }

    public class Range<T>
    {
        public T? MinVal { get; set; }
        public T? MaxVal { get; set; }
    }
}
