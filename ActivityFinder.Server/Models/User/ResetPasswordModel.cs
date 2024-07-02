using System.ComponentModel.DataAnnotations;

namespace ActivityFinder.Server.Models
{
    public class ResetPasswordModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email wymagany")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Token wymagany")]
        public string? Token { get; set; }

        [Required(ErrorMessage = "Hasło wymagane")]
        public string? NewPassword { get; set; }
    }
}
