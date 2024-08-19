namespace MIS_Healthcare.UI.DTOs.Appointment
{
    public class AppointmentToUpdate
    {
        public int AppointmentID { get; set; }
        public string Problem { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }
        public string AppointmentStatus { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
