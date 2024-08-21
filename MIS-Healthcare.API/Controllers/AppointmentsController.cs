using Microsoft.AspNetCore.Mvc;
using MIS_Healthcare.API.Data.DTOs.Appointment;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Repository.Interface;

namespace MIS_Healthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly iAppointmentRepo _appointmentRepository;
        private readonly iDoctorRepo _doctorRepository;
        private readonly iPatientRepo _patientRepository;

        public AppointmentsController(iAppointmentRepo appointmentRepository, iDoctorRepo doctorRepository, iPatientRepo patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentToRead>>> GetAppointments()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAllAppointmentsAsync();
                var appointmentDtos = appointments.Select(a => new AppointmentToRead
                {
                    AppointmentID = a.AppointmentID,
                    Problem = a.Problem,
                    PatientID = a.PatientID,
                    PatientName = $"{a.Patient.FirstName} {a.Patient.LastName}", 
                    DoctorName = $"{a.Doctor.FirstName} {a.Doctor.LastName}",
                    DoctorID = a.DoctorID,
                    DoctorType = a.Doctor.DoctorType,
                    Qualification = a.Doctor.DoctorType,
                    DoctorFees = a.DoctorFees,
                    PaymentMode = a.PaymentMode,
                    PaymentStatus= a.PaymentStatus ?? "Pending",
                    AppointmentStatus = a.AppointmentStatus,
                    AppointmentDate = a.AppointmentDate
                }).ToList();

                return Ok(appointmentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching appointments.", details = ex.Message });
            }
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentToRead>> GetAppointment(int id)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id);
                if (appointment == null)
                {
                    return NotFound();
                }

                var appointmentDto = new AppointmentToRead
                {
                    AppointmentID = appointment.AppointmentID,
                    Problem = appointment.Problem,
                    PatientID = appointment.PatientID,
                    PatientName = $"{appointment.Patient.FirstName} {appointment.Patient.LastName}", 
                    DoctorName = $"{appointment.Doctor.FirstName} {appointment.Doctor.LastName}",
                    DoctorID = appointment.DoctorID,
                    DoctorType = appointment.Doctor.DoctorType,
                    Qualification = appointment.Doctor.Qualification,
                    DoctorFees = appointment.Doctor.EntryCharge,
                    PaymentStatus = appointment.PaymentStatus ?? "Not Paid",
                    PaymentMode = appointment.PaymentMode,
                    AppointmentStatus = appointment.AppointmentStatus,
                    AppointmentDate = appointment.AppointmentDate
                };

                return Ok(appointmentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the appointment.", details = ex.Message });
            }
        }

        // POST: api/Appointments
        [HttpPost]
        public async Task<ActionResult<AppointmentToRead>> PostAppointment([FromBody] AppointmentToRegister appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var appointment = new Appointment
                {
                    Problem = appointmentDto.Problem,
                    PatientID = appointmentDto.PatientID,
                    DoctorID = appointmentDto.DoctorID,
                    PaymentMode = appointmentDto.PaymentMode,
                    PaymentStatus=appointmentDto.PaymentStatus,
                    AppointmentStatus = appointmentDto.AppointmentStatus,
                    AppointmentDate = appointmentDto.AppointmentDate
                };

                await _appointmentRepository.AddAppointmentAsync(appointment);

                // Fetch the Patient and Doctor details for full names
                var patient = await _patientRepository.GetPatientByIdAsync(appointment.PatientID);
                var doctor = await _doctorRepository.GetDoctorByIdAsync(appointment.DoctorID);

                var createdAppointmentDto = new AppointmentToRead
                {
                    AppointmentID = appointment.AppointmentID,
                    Problem = appointment.Problem,
                    PatientID = appointment.PatientID,
                    PatientName = $"{patient.FirstName} {patient.LastName}",
                    DoctorName = $"{doctor.FirstName} {doctor.LastName}",
                    DoctorID = appointment.DoctorID,
                    DoctorType = appointment.Doctor.DoctorType,
                    Qualification = appointment.Doctor.DoctorType,
                    DoctorFees = appointment.DoctorFees,
                    PaymentStatus = appointment.PaymentStatus,
                    PaymentMode = appointment.PaymentMode,
                    AppointmentStatus = appointment.AppointmentStatus,
                    AppointmentDate = appointment.AppointmentDate
                };

                return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentID }, createdAppointmentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the appointment.", details = ex.Message });
            }
        }

        // PUT: api/Appointments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, [FromBody] AppointmentToUpdate appointmentDto)
        {
            if (id != appointmentDto.AppointmentID)
            {
                return BadRequest();
            }

            try
            {
                var appointment = new Appointment
                {
                    AppointmentID = appointmentDto.AppointmentID,
                    Problem = appointmentDto.Problem,
                    PatientID = appointmentDto.PatientID,
                    DoctorID = appointmentDto.DoctorID,
                    PaymentStatus = appointmentDto.PaymentStatus,
                    PaymentMode = appointmentDto.PaymentMode,
                    AppointmentStatus = appointmentDto.AppointmentStatus,
                    AppointmentDate = appointmentDto.AppointmentDate
                };

                var updated = await _appointmentRepository.UpdateAppointmentAsync(appointment);
                if (!updated)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the appointment.", details = ex.Message });
            }
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var deleted = await _appointmentRepository.DeleteAppointmentAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the appointment.", details = ex.Message });
            }
        }
    }
}
