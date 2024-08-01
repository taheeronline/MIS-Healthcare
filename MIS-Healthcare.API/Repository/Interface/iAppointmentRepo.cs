using MIS_Healthcare.API.Data.Models;

namespace MIS_Healthcare.API.Repository.Interface
{
    public interface iAppointmentRepo
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task AddAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAppointmentAsync(int id);
    }
}
