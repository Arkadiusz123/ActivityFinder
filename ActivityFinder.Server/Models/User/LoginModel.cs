using System.ComponentModel.DataAnnotations;

namespace ActivityFinder.Server.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nazwa wymagana")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Hasło wymagane")]
        public string? Password { get; set; }
    }
}
