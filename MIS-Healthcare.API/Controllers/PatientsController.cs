using Microsoft.AspNetCore.Mvc;
using MIS_Healthcare.API.Data.DTOs.Patient;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Middleware;
using MIS_Healthcare.API.Repository.Interface;

namespace MIS_Healthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly iPatientRepo _patientRepository;

        public PatientsController(iPatientRepo patientRepository)
        {
            _patientRepository = patientRepository;
        }

        // GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientToRead>>> GetPatients()
        {
            try
            {
                var patients = await _patientRepository.GetAllPatientsAsync();
                var patientDtos = patients.Select(p => new PatientToRead
                {
                    PatientID = p.PatientID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    ContactNumber = p.ContactNumber,
                    Age = p.Age,
                    EmailID = p.EmailID,
                    BloodGroup = p.BloodGroup,
                    Address = p.Address
                }).ToList();

                return Ok(patientDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching patients.", details = ex.Message });
            }
        }

        [HttpGet("PatientList")]
        public async Task<IActionResult> GetPatientList()
        {
            try
            {
                var patients = await _patientRepository.GetAllPatientsAsync();
                var doctorNameDtos = patients.Select(d => new PatientList
                {
                    PatientID = d.PatientID,
                    FullName = $"{d.FirstName} {d.LastName}"
                }).ToList();

                return Ok(doctorNameDtos);
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching patient names.", details = ex.Message });
            }
        }


        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientToRead>> GetPatient(int id)
        {
            try
            {
                var patient = await _patientRepository.GetPatientByIdAsync(id);
                if (patient == null)
                {
                    return NotFound();
                }

                var patientDto = new PatientToRead
                {
                    PatientID = patient.PatientID,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Gender = patient.Gender,
                    ContactNumber = patient.ContactNumber,
                    Age = patient.Age,
                    EmailID = patient.EmailID,
                    BloodGroup = patient.BloodGroup,
                    Address = patient.Address
                };

                return Ok(patientDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the patient.", details = ex.Message });
            }
        }

        // POST: api/Patients
        [HttpPost]
        public async Task<ActionResult<PatientToRead>> PostPatient([FromBody] PatientToRegister patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var patient = new Patient
                {
                    FirstName = patientDto.FirstName,
                    LastName = patientDto.LastName,
                    Gender = patientDto.Gender,
                    ContactNumber = patientDto.ContactNumber,
                    Age = patientDto.Age,
                    EmailID = patientDto.EmailID,
                    BloodGroup = patientDto.BloodGroup,
                    Address = patientDto.Address
                };

                await _patientRepository.AddPatientAsync(patient);

                var createdPatientDto = new PatientToRead
                {
                    PatientID = patient.PatientID,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Gender = patient.Gender,
                    ContactNumber = patient.ContactNumber,
                    Age = patient.Age,
                    EmailID = patient.EmailID,
                    BloodGroup = patient.BloodGroup,
                    Address = patient.Address
                };

                return CreatedAtAction(nameof(GetPatient), new { id = patient.PatientID }, createdPatientDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the patient.", details = ex.Message });
            }
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, [FromBody] PatientToUpdate patientDto)
        {
            if (id != patientDto.PatientID)
            {
                return BadRequest();
            }

            try
            {
                var patient = new Patient
                {
                    PatientID = patientDto.PatientID,
                    FirstName = patientDto.FirstName,
                    LastName = patientDto.LastName,
                    Gender = patientDto.Gender,
                    ContactNumber = patientDto.ContactNumber,
                    Age = patientDto.Age,
                    EmailID = patientDto.EmailID,
                    BloodGroup = patientDto.BloodGroup,
                    Address = patientDto.Address
                };

                var updated = await _patientRepository.UpdatePatientAsync(patient);
                if (!updated)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the patient.", details = ex.Message });
            }
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            try
            {
                var deleted = await _patientRepository.DeletePatientAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the patient.", details = ex.Message });
            }
        }
    }
}
