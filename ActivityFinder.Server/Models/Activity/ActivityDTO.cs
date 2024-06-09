
using System.ComponentModel.DataAnnotations;

namespace ActivityFinder.Server.Models;

public class ActivityDTO
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Nazwa wymagana")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Adres wymagany")]
    public required AddressDTO Address { get; set; }

    [Required(ErrorMessage = "Opis wymagany")]
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "Data wymagana")]
    public DateTime? Date { get; set; }

    public string? OtherInfo { get; set; }

    //public required string Category { get; set; }
}
