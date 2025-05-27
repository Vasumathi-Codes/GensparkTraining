// public class AppointmentRepository : IRepository<int, Appointment>
// {
//     private List<Appointment> _appointments = new List<Appointment>();
//     private int _idCounter = 1;

//     public List<Appointment> GetAll() {
//         return _appointments;
//     }

//     public Appointment? GetById(int id) {
//         return _appointments.FirstOrDefault(a => a.Id == id);
//     }

//     public void Add(Appointment appointment)
//     {
//         appointment.Id = _idCounter++;
//         _appointments.Add(appointment);
//     }

//     public void Update(Appointment appointment)
//     {
//         var existing = _appointments.FirstOrDefault(a => a.Id == appointment.Id);
//         if (existing != null)
//         {
//             _appointments.Remove(existing);
//             _appointments.Add(appointment);
//         }
//     }

//     public void Delete(int id)
//     {
//         _appointments.RemoveAll(a => a.Id == id);
//     }
// }
