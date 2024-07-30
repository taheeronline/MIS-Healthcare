namespace MIS_Healthcare.API.Data.Models
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public int Age { get; set; }
        public string EmailID { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
    }
}
