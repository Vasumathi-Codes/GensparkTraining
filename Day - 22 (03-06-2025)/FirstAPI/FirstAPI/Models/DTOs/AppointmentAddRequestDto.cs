
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAPI.Models.DTOs
{
    public class AppointmentAddRequestDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Status {get; set;} = string.Empty;
    }
}