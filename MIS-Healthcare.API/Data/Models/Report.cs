using System.ComponentModel.DataAnnotations;

namespace MIS_Healthcare.API.Data.Models
{
    public class Report
    {
        [Key]
        public int ReportID { get; set; }
        public int AppointmentID { get; set; }
        public Appointment Appointment { get; set; }
        public int PatientID { get; set; }
        public Patient Patient { get; set; }
        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }
        public string MedicinePrescribed { get; set; }
        public string DoctorComment { get; set; }
    }
}
