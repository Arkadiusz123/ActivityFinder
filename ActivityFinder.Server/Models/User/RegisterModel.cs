﻿using System.ComponentModel.DataAnnotations;

namespace ActivityFinder.Server.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Nazwa wymagana")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email wymagany")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Hasło wymagane")]
        public string? Password { get; set; }
    }
}
