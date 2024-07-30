using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Middleware;
using MIS_Healthcare.API.Repository.Interface;

namespace MIS_Healthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly iDoctorRepo _doctorRepository;

        public DoctorsController(iDoctorRepo doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            try
            {
                var doctors = await _doctorRepository.GetAllDoctorsAsync();
                return Ok(doctors);
            }
            catch (RepositoryException ex)
            {
                // Handle repository specific exceptions if needed
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, new { message = "An error occurred while fetching doctors.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            try
            {
                var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
                if (doctor == null)
                {
                    return NotFound();
                }
                return Ok(doctor);
            }
            catch (RepositoryException ex)
            {
                // Handle repository specific exceptions if needed
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, new { message = "An error occurred while fetching the doctor.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] Doctor doctor)
        {
            try
            {
                await _doctorRepository.AddDoctorAsync(doctor);
                return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.DoctorID }, doctor);
            }
            catch (RepositoryException ex)
            {
                // Handle repository specific exceptions if needed
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, new { message = "An error occurred while adding the doctor.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            if (id != doctor.DoctorID)
            {
                return BadRequest();
            }

            try
            {
                await _doctorRepository.UpdateDoctorAsync(doctor);
                return NoContent();
            }
            catch (RepositoryException ex)
            {
                // Handle repository specific exceptions if needed
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, new { message = "An error occurred while updating the doctor.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                await _doctorRepository.DeleteDoctorAsync(id);
                return NoContent();
            }
            catch (RepositoryException ex)
            {
                // Handle repository specific exceptions if needed
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, new { message = "An error occurred while deleting the doctor.", details = ex.Message });
            }
        }
    }
}
