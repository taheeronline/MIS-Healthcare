using Microsoft.EntityFrameworkCore;
using MIS_Healthcare.API.Data;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Middleware;
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
            try
            {
                var appointments= await _context.Appointments
                                                .Include(a=>a.Doctor)
                                                .Include(a=>a.Patient)
                                                .Where(a=>a.AppointmentStatus!="Complete")
                                                .ToListAsync();
                return appointments;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error fetching all appointment.", ex);
            }
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            try
            {
                var appointment=await _context.Appointments
                        .Include(a => a.Patient)
                        .Include(a => a.Doctor)
                        .FirstOrDefaultAsync(a => a.AppointmentID == id);

                return appointment;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error fetching appointment with ID {id}.", ex);
            }
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            try
            {
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error adding appointment.", ex);
            }
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
                if (!AppointmentExists(appointment.AppointmentID))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error fetching all appointment.", ex);
            }
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            try
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
            catch (Exception ex)
            {
                throw new RepositoryException("Error updating appointment.", ex);
            }
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }
    }
}
