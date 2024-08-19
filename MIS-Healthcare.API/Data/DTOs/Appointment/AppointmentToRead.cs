namespace MIS_Healthcare.API.Data.DTOs.Appointment
{
    public class AppointmentToRead
    {
        public int AppointmentID { get; set; }
        public string Problem { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } 
        public string DoctorName { get; set; }
        public int DoctorID { get; set; }
        public string DoctorType { get; set; }
        public string Qualification { get; set; }
        public int DoctorFees { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMode { get; set; }
        public string AppointmentStatus { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
