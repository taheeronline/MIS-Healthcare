namespace MIS_Healthcare.API.Data.DTOs.Appointment
{
    public class AppointmentToRegister
    {
        public string Problem { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }
        public string AppointmentStatus { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
    