using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Data;
using MIS_Healthcare.API.Middleware;
using MIS_Healthcare.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MIS_Healthcare.API.Repository.Implementation
{
    public class FeedbackRepo:iFeedbackRepo
    {
        private readonly HealthcareContext _context;

        public FeedbackRepo(HealthcareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbackAsync()
        {
            try
            {
                return await _context.Feedbacks
                                     .Include(f => f.Patient)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error fetching all feedback.", ex);
            }
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int id)
        {
            try
            {
                return await _context.Feedbacks
                                     .Include(f => f.Patient)
                                     .FirstOrDefaultAsync(f => f.FeedbackID == id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error fetching feedback with ID {id}.", ex);
            }
        }

        public async Task AddFeedbackAsync(Feedback feedback)
        {
            try
            {
                await _context.Feedbacks.AddAsync(feedback);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error adding feedback.", ex);
            }
        }

        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            try
            {
                _context.Feedbacks.Update(feedback);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error updating feedback.", ex);
            }
        }

        public async Task DeleteFeedbackAsync(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback != null)
                {
                    _context.Feedbacks.Remove(feedback);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error deleting feedback.", ex);
            }
        }
    }
}
