using MIS_Healthcare.API.Data.Models;

namespace MIS_Healthcare.API.Repository.Interface
{
    public interface iDoctorRepo
    {
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task<Doctor> GetDoctorByIdAsync(int id);
        Task AddDoctorAsync(Doctor doctor);
        Task UpdateDoctorAsync(Doctor doctor);
        Task DeleteDoctorAsync(int id);
    }
}
