using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MIS_Healthcare.API.Data.DTOs.Doctor;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Middleware;
using MIS_Healthcare.API.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                var doctorDtos = doctors.Select(d => new DoctorToRead
                {
                    DoctorID = d.DoctorID,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Gender = d.Gender,
                    ContactNumber = d.ContactNumber,
                    Age = d.Age,
                    EntryCharge = d.EntryCharge,
                    Qualification = d.Qualification,
                    DoctorType = d.DoctorType,
                    EmailID = d.EmailID
                }).ToList();
                return Ok(doctorDtos);
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
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

                var doctorDto = new DoctorToRead
                {
                    DoctorID = doctor.DoctorID,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Gender = doctor.Gender,
                    ContactNumber = doctor.ContactNumber,
                    Age = doctor.Age,
                    EntryCharge = doctor.EntryCharge,
                    Qualification = doctor.Qualification,
                    DoctorType = doctor.DoctorType,
                    EmailID = doctor.EmailID
                };

                return Ok(doctorDto);
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the doctor.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] DoctorToRegister doctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var doctor = new Doctor
                {
                    FirstName = doctorDto.FirstName,
                    LastName = doctorDto.LastName,
                    Gender = doctorDto.Gender,
                    ContactNumber = doctorDto.ContactNumber,
                    Age = doctorDto.Age,
                    EntryCharge = doctorDto.EntryCharge,
                    Qualification = doctorDto.Qualification,
                    DoctorType = doctorDto.DoctorType,
                    EmailID = doctorDto.EmailID
                };

                await _doctorRepository.AddDoctorAsync(doctor);
                var createdDoctorDto = new DoctorToRead
                {
                    DoctorID = doctor.DoctorID,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Gender = doctor.Gender,
                    ContactNumber = doctor.ContactNumber,
                    Age = doctor.Age,
                    EntryCharge = doctor.EntryCharge,
                    Qualification = doctor.Qualification,
                    DoctorType = doctor.DoctorType,
                    EmailID = doctor.EmailID
                };

                return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.DoctorID }, createdDoctorDto);
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the doctor.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorToUpdate doctorDto)
        {
            if (id != doctorDto.DoctorID)
            {
                return BadRequest();
            }

            try
            {
                var doctor = new Doctor
                {
                    DoctorID = doctorDto.DoctorID,
                    FirstName = doctorDto.FirstName,
                    LastName = doctorDto.LastName,
                    Gender = doctorDto.Gender,
                    ContactNumber = doctorDto.ContactNumber,
                    Age = doctorDto.Age,
                    EntryCharge = doctorDto.EntryCharge,
                    Qualification = doctorDto.Qualification,
                    DoctorType = doctorDto.DoctorType,
                    EmailID = doctorDto.EmailID
                };

                await _doctorRepository.UpdateDoctorAsync(doctor);
                return NoContent();
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
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
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the doctor.", details = ex.Message });
            }
        }
    }
}
