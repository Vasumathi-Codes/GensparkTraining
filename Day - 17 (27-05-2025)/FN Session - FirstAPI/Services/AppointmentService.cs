// public class AppointmentService : IAppointmentService
// {
//     private readonly IRepository<int, Appointment> _repo;

//     public AppointmentService(IRepository<int, Appointment> repo)
//     {
//         _repo = repo;
//     }

//     public List<Appointment> GetAllAppointments()
//     {
//         return _repo.GetAll();
//     }

//     public Appointment? GetAppointment(int id)
//     {
//         return _repo.GetById(id);
//     }

//     public void BookAppointment(Appointment appointment)
//     {
//         _repo.Add(appointment);
//     }

//     public bool UpdateAppointment(Appointment appointment)
//     {
//         var existing = _repo.GetById(appointment.Id);
//         if (existing == null)
//             return false;

//         _repo.Update(appointment);
//         return true;
//     }

//     public bool CancelAppointment(int id)
//     {
//         var existing = _repo.GetById(id);
//         if (existing == null)
//             return false;

//         _repo.Delete(id);
//         return true;
//     }
// }
