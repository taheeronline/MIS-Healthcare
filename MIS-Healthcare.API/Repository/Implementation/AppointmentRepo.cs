using Microsoft.EntityFrameworkCore;
using MIS_Healthcare.API.Data;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Repository.Interface;

namespace MIS_Healthcare.API.Repository.Implementation
{
    public class AppointmentRepo:iAppointmentRepo
    {
        private readonly HealthcareContext _context;

        public AppointmentRepo(HealthcareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentID == id);
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            _context.Entry(appointment).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AppointmentExists(appointment.AppointmentID))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return false;
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> AppointmentExists(int id)
        {
            return await _context.Appointments.AnyAsync(e => e.AppointmentID == id);
        }
    }
}
