using Microsoft.EntityFrameworkCore;
using MIS_Healthcare.API.Data;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Repository.Interface;

namespace MIS_Healthcare.API.Repository.Implementation
{
    public class ReportRepo:iReportRepo
    {
        private readonly HealthcareContext _context;

        public ReportRepo(HealthcareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _context.Reports
                .Include(r => r.Appointment)
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .ToListAsync();
        }

        public async Task<Report> GetReportByIdAsync(int id)
        {
            return await _context.Reports
                .Include(r => r.Appointment)
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .FirstOrDefaultAsync(r => r.ReportID == id);
        }

        public async Task AddReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
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
                if (!await ReportExists(report.ReportID))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteReportAsync(int id)
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

        private async Task<bool> ReportExists(int id)
        {
            return await _context.Reports.AnyAsync(e => e.ReportID == id);
        }
    }
}
