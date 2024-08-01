using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Repository.Interface;

namespace MIS_Healthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly iAppointmentRepo _appointmentRepository;

        public AppointmentsController(iAppointmentRepo appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return Ok(await _appointmentRepository.GetAllAppointmentsAsync());
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        // POST: api/Appointments
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            await _appointmentRepository.AddAppointmentAsync(appointment);
            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentID }, appointment);
        }

        // PUT: api/Appointments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
        {
            if (id != appointment.AppointmentID)
            {
                return BadRequest();
            }

            var updated = await _appointmentRepository.UpdateAppointmentAsync(appointment);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var deleted = await _appointmentRepository.DeleteAppointmentAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
