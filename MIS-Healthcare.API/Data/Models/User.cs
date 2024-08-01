using System.ComponentModel.DataAnnotations;

namespace MIS_Healthcare.API.Data.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
    }
}
