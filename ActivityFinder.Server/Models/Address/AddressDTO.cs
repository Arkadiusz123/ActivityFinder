using System.ComponentModel.DataAnnotations;

namespace ActivityFinder.Server.Models
{
    public class AddressDTO
    {
        [Required(ErrorMessage = "Adres wymagany")]
        public string? OsmId { get; set; }
        public string? DisplayName { get; set; }
    }
}
