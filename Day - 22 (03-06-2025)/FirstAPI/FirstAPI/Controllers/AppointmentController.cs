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

    [HttpPost]
    public async Task<ActionResult<Appointment>> BookAppointment([FromBody] AppointmentAddRequestDto appointmentDto)
    {
        var createdAppointment = await _appointmentService.BookAppointment(appointmentDto);
        if (createdAppointment == null)
        {
            return BadRequest("Unable to create appointment");
        }

        var uri = $"api/appointment/{createdAppointment.AppointmentNumber}"; 
        return Created(uri, createdAppointment);
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
