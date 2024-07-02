using System.ComponentModel.DataAnnotations;

namespace ActivityFinder.Server.Models
{
    public class ForgotPasswordModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email wymagany")]
        public string? Email { get; set; }
    }
}
