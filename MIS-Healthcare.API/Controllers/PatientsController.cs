using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MIS_Healthcare.API.Data.Models;
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
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return Ok(await _patientRepository.GetAllPatientsAsync());
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // POST: api/Patients
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            await _patientRepository.AddPatientAsync(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.PatientID }, patient);
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.PatientID)
            {
                return BadRequest();
            }

            var updated = await _patientRepository.UpdatePatientAsync(patient);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var deleted = await _patientRepository.DeletePatientAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
