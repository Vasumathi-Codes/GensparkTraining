// using Microsoft.AspNetCore.Mvc;

// [ApiController]
// [Route("api/[controller]")]
// public class AppointmentController : ControllerBase
// {
//     private readonly IAppointmentService _appointmentService;

//     public AppointmentController(IAppointmentService appointmentService)
//     {
//         _appointmentService = appointmentService;
//     }

//     [HttpGet]
//     public ActionResult<List<Appointment>> GetAll()
//     {
//         var appointments = _appointmentService.GetAllAppointments();
//         return Ok(appointments);
//     }

//     [HttpGet("{id}")]
//     public ActionResult<Appointment> GetById(int id)
//     {
//         var appointment = _appointmentService.GetAppointment(id);
//         if (appointment == null)
//             return NotFound($"Appointment with id: {id} not found");

//         return Ok(appointment);
//     }

//     [HttpPost]
//     public ActionResult BookAppointment([FromBody] Appointment appointment)
//     {
//         _appointmentService.BookAppointment(appointment);
//         return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
//     }

//     [HttpPut("{id}")]
//     public ActionResult UpdateAppointment(int id, [FromBody] Appointment appointment)
//     {
//         if (id != appointment.Id)
//             return BadRequest("ID mismatch");

//         var updated = _appointmentService.UpdateAppointment(appointment);
//         if (!updated)
//             return NotFound($"Appointment with id: {id} not found");

//         return NoContent();
//     }

//     [HttpDelete("{id}")]
//     public ActionResult CancelAppointment(int id)
//     {
//         var canceled = _appointmentService.CancelAppointment(id);
//         if (!canceled)
//             return NotFound($"Appointment with id: {id} not found");

//         return NoContent();
//     }
// }
