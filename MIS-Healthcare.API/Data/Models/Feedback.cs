namespace MIS_Healthcare.API.Data.Models
{
    public class Feedback
    {
        public int PatientID { get; set; }
        public Patient Patient { get; set; }
        public int Points { get; set; }
        public string DocNature { get; set; }
        public string Location { get; set; }
        public string PatientComment { get; set; }
    }
}
