// using FirstAPI.Interfaces;
// using FirstAPI.Models;
// using FirstAPI.DTOs;
// using Microsoft.AspNetCore.Mvc;

// namespace FirstAPI.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class DoctorsController : ControllerBase
//     {
//         private readonly IDoctorService _doctorService;

//         public DoctorsController(IDoctorService doctorService)
//         {
//             _doctorService = doctorService;
//         }

//         // POST: api/Doctors
//         [HttpPost]
//         public async Task<ActionResult<Doctor>> AddDoctor([FromBody] DoctorAddRequestDto doctorDto)
//         {
//             try
//             {
//                 var addedDoctor = await _doctorService.AddDoctor(doctorDto);
//                 return CreatedAtAction(nameof(GetDoctorByName), new { name = addedDoctor.Name }, addedDoctor);
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(ex.Message);
//             }
//         }

//         // GET: api/Doctors/name/{name}
//         [HttpGet("name/{name}")]
//         public async Task<ActionResult<Doctor>> GetDoctorByName(string name)
//         {
//             try
//             {
//                 var doctor = await _doctorService.GetDoctByName(name);
//                 return Ok(doctor);
//             }
//             catch (Exception ex)
//             {
//                 return NotFound(ex.Message);
//             }
//         }

//         // GET: api/Doctors/speciality/{speciality}
//         [HttpGet("speciality/{speciality}")]
//         public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctorsBySpeciality(string speciality)
//         {
//             try
//             {
//                 var doctors = await _doctorService.GetDoctorsBySpeciality(speciality);
//                 return Ok(doctors);
//             }
//             catch (Exception ex)
//             {
//                 return NotFound(ex.Message);
//             }
//         }
//     }
// }
