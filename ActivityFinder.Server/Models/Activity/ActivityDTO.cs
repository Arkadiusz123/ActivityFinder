
namespace ActivityFinder.Server.Models;

public class ActivityDTO
{
    public int? Id { get; set; }
    public required AddressDTO Address { get; set; }
    public required string Description { get; set; }
    //public required string Category { get; set; }
    public required DateTime Date { get; set; }
}
