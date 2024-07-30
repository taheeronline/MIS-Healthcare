using Microsoft.EntityFrameworkCore;
using MIS_Healthcare.API.Data;
using MIS_Healthcare.API.Data.Models;
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
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task AddPatientAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
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
        }

        public async Task<bool> DeletePatientAsync(int id)
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

        private async Task<bool> PatientExists(int id)
        {
            return await _context.Patients.AnyAsync(e => e.PatientID == id);
        }
    }
}
