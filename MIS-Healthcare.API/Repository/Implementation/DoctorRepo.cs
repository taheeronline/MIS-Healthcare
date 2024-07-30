using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Data;
using MIS_Healthcare.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using MIS_Healthcare.API.Middleware;

namespace MIS_Healthcare.API.Repository.Implementation
{
    public class DoctorRepo : iDoctorRepo
    {
        private readonly HealthcareContext _context;

        public DoctorRepo(HealthcareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            try
            {
                return await _context.Doctors.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error fetching all doctors.", ex);
            }
        }

        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
            try
            {
                return await _context.Doctors.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error fetching doctor with ID {id}.", ex);
            }
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            try
            {
                await _context.Doctors.AddAsync(doctor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error adding doctor.", ex);
            }
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            try
            {
                _context.Doctors.Update(doctor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error updating doctor.", ex);
            }
        }

        public async Task DeleteDoctorAsync(int id)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(id);
                if (doctor != null)
                {
                    _context.Doctors.Remove(doctor);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error deleting doctor.", ex);
            }
        }
    }

}
