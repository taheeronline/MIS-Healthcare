﻿namespace MIS_Healthcare.API.Data.DTOs.Report
{
    public class ReportToRegister
    {
        public int ReportID { get; set; }
        public int AppointmentID { get; set; }
        public string MedicinePrescribed { get; set; }
        public string DoctorComment { get; set; }
    }
}
