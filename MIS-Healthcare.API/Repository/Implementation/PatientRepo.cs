using Microsoft.EntityFrameworkCore;
using MIS_Healthcare.API.Data;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Middleware;
using MIS_Healthcare.API.Repository.Interface;

namespace MIS_Healthcare.API.Repository.Implementation
{
    public class PatientRepo:iPatientRepo
    {
        private readonly HealthcareContext _context;

        public PatientRepo(HealthcareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            try
            {
                return await _context.Patients.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error fetching all feedback.", ex);
            }
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            try
            {
                return await _context.Patients.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error fetching all feedback.", ex);
            }
        }

        public async Task AddPatientAsync(Patient patient)
        {
            try
            {
                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error fetching all feedback.", ex);
            }
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            _context.Entry(patient).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PatientExists(patient.PatientID))
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
                throw new RepositoryException("Error fetching all feedback.", ex);
            }
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                {
                    return false;
                }

                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error fetching all feedback.", ex);
            }
        }

        private async Task<bool> PatientExists(int id)
        {
            return await _context.Patients.AnyAsync(e => e.PatientID == id);
        }
    }
}
