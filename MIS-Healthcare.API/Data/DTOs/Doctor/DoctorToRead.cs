namespace MIS_Healthcare.API.Data.DTOs.Doctor
{
    public class DoctorToRead
    {
        public int DoctorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public int Age { get; set; }
        public int EntryCharge { get; set; }
        public string Qualification { get; set; }
        public string DoctorType { get; set; }
        public string EmailID { get; set; }
    }
}
