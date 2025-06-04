using Microsoft.AspNetCore.Mvc;
using FirstAPI.Models;
using FirstAPI.Interfaces;
using FirstAPI.Models.DTOs;
using FirstAPI.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("appointmentNo")]
    public async Task<ActionResult<Appointment>> GetAppointmentByAppointmentNumber(string appointmentNo) {
        var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentNo);
        if(appointment == null)
            return NotFound($"Appointment with no: {appointmentNo} not found");
        return Ok(appointment);
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Appointment>>> GetAllAppointments() {
        var appointments = await _appointmentService.GetAllAppointments();
        return Ok(appointments);
    }

    [HttpPost]
    public async Task<ActionResult<Appointment>> BookAppointment([FromBody] AppointmentAddRequestDto appointmentDto)
    {
        try {
            var createdAppointment = await _appointmentService.BookAppointment(appointmentDto);
            if (createdAppointment == null)
            {
                return BadRequest("Unable to create appointment");
            }

            var uri = $"api/appointment/{createdAppointment.AppointmentNumber}"; 
            return Created(uri, createdAppointment);
        } catch(Exception ex) {
            return StatusCode(500, $"An unexpected error occurred.{ex.Message}");
        }
    }

    [HttpPut("{appointmentNo}")]
    public async Task<IActionResult> UpdateAppointment(string appointmentNo, [FromBody] string status)
    {
        try {
            var updatedAppointment = await _appointmentService.UpdateAppointmentStatusAsync(appointmentNo, status);
            if (updatedAppointment == null)
            {
                return NotFound($"Appointment with number {appointmentNo} not found.");
            }
            return Ok(updatedAppointment);
        } catch(Exception ex) {
            return StatusCode(500, $"An unexpected error occurred.{ex.Message}");
        }
    }

    [Authorize(Roles = "Doctor")]
    [Authorize(Policy = "ExperiencedDoctorOnly")]
    [HttpDelete("{appointmentNo}")]
    public async Task<IActionResult> CancelAppointment(string appointmentNo)
    {
        try
        {
            var success = await _appointmentService.CancelAppointment(appointmentNo, User);
            return Ok("Appointment cancelled successfully.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

}
