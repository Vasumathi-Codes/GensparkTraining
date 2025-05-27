// using Microsoft.AspNetCore.Mvc;

// [ApiController]
// [Route("/api/[controller]")]
// public class PatientController : ControllerBase
// {
//     static List<Patient> patients = new List<Patient>
//     {
//         new Patient { Id = 1, Name = "John", Age = 30, Disease = "Flu"},
//         new Patient { Id = 2, Name = "Walter", Age = 25, Disease = "Cancer"},
//     };

//     [HttpGet]
//     public ActionResult<IEnumerable<Patient>> GetPatients()
//     {
//         return Ok(patients);
//     }

//     [HttpPost]
//     public ActionResult<Patient> PostPatient([FromBody] Patient patient)
//     {
//         if (patients.Any(p => p.Id == patient.Id))
//         {
//             return BadRequest($"Patient with Id {patient.Id} already exists");
//         }
//         patients.Add(patient);
//         return Created("", patient);
//     }

//     [HttpPut("{id}")]
//     public ActionResult<Patient> UpdatePatient(int id, [FromBody] Patient updatedPatient)
//     {
//         var patient = patients.FirstOrDefault(p => p.Id == id);
//         if (patient == null)
//         {
//             return NotFound($"Patient with Id {id} not found");
//         }

//         patient.Name = updatedPatient.Name;
//         patient.Age = updatedPatient.Age;
//         patient.Disease = updatedPatient.Disease;

//         return Ok(patient);
//     }

//     [HttpDelete("{id}")]
//     public ActionResult<string> DeletePatient(int id)
//     {
//         var patient = patients.FirstOrDefault(p => p.Id == id);
//         if (patient == null)
//         {
//             return NotFound($"Patient with Id {id} not found");
//         }

//         patients.Remove(patient);
//         return Ok("Patient record deleted successfully");
//     }
// }
