using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // POST: api/Patients
        [HttpPost]
        public async Task<ActionResult<Patient>> AddPatient([FromBody] PatientAddRequestDto patientDto)
        {
            try
            {
                var createdPatient = await _patientService.AddPatient(patientDto);
                return Ok(createdPatient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
