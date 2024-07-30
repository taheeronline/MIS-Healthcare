namespace MIS_Healthcare.API.Data.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public string Problem { get; set; }
        public int PatientID { get; set; }
        public Patient Patient { get; set; }
        public string DoctorName { get; set; }
        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }
        public string DoctorType { get; set; }
        public string Qualification { get; set; }
        public int DoctorFees { get; set; }
        public string PaymentStatus { get; set; }
        public string AppointmentStatus { get; set; }
    }
}
