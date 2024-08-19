using Microsoft.EntityFrameworkCore;
using MIS_Healthcare.API.Data;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Middleware;
using MIS_Healthcare.API.Repository.Interface;

namespace MIS_Healthcare.API.Repository.Implementation
{
    public class ReportRepo : iReportRepo
    {
        private readonly HealthcareContext _context;

        public ReportRepo(HealthcareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {

            try
            {
                return await _context.Reports
                       .Include(r => r.Appointment)
                       .Include(r => r.Patient)
                       .Include(r => r.Doctor)
                       .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error fetching all reports.", ex);
            }
        }

        public async Task<Report> GetReportByIdAsync(int id)
        {
            try
            {
                return await _context.Reports
                        .Include(r => r.Appointment)
                        .Include(r => r.Patient)
                        .Include(r => r.Doctor)
                        .FirstOrDefaultAsync(r => r.ReportID == id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error fetching report with ID {id}.", ex);
            }
        }
            public async Task AddReportAsync(Report report)
            {
                try
                {
                    _context.Reports.Add(report);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new RepositoryException("Error adding report.", ex);
                }
            }

            public async Task<bool> UpdateReportAsync(Report report)
            {
                _context.Entry(report).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.ReportID))
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
                    throw new RepositoryException("Error updating report.", ex);
                }
            }

            public async Task<bool> DeleteReportAsync(int id)
            {
                try
                {
                    var report = await _context.Reports.FindAsync(id);
                    if (report == null)
                    {
                        return false;
                    }

                    _context.Reports.Remove(report);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new RepositoryException("Error deleting report.", ex);
                }
            }

            private bool ReportExists(int id)
            {
                return _context.Reports.Any(e => e.ReportID == id);
            }
    } 
}
