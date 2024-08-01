using MIS_Healthcare.API.Data.Models;

namespace MIS_Healthcare.API.Repository.Interface
{
    public interface iReportRepo
    {
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report> GetReportByIdAsync(int id);
        Task AddReportAsync(Report report);
        Task<bool> UpdateReportAsync(Report report);
        Task<bool> DeleteReportAsync(int id);
    }
}
