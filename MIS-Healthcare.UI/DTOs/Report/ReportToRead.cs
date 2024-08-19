namespace MIS_Healthcare.UI.DTOs.Report
{
    public class ReportToRead
    {
        public int ReportID { get; set; }
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } // Derived from Patient entity
        public int DoctorID { get; set; }
        public string DoctorName { get; set; } // Derived from Doctor entity
        public string MedicinePrescribed { get; set; }
        public string DoctorComment { get; set; }
    }
}
