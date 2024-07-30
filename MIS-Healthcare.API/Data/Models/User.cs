using System.ComponentModel.DataAnnotations;

namespace MIS_Healthcare.API.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public string UserType { get; set; }
        public string Password { get; set; }
    }
}
