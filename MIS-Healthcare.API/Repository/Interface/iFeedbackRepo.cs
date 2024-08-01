using MIS_Healthcare.API.Data.Models;

namespace MIS_Healthcare.API.Repository.Interface
{
    public interface iFeedbackRepo
    {
        Task<IEnumerable<Feedback>> GetAllFeedbackAsync();
        Task<Feedback> GetFeedbackByIdAsync(int id);
        Task AddFeedbackAsync(Feedback feedback);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task DeleteFeedbackAsync(int id);
    }
}
