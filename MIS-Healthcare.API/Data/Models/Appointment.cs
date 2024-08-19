using System.ComponentModel.DataAnnotations;

namespace MIS_Healthcare.API.Data.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }
        public string Problem { get; set; }
        public int PatientID { get; set; }
        public Patient Patient { get; set; }
        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }
        public int DoctorFees { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMode { get; set; }
        public string AppointmentStatus { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
