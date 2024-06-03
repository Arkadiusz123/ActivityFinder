
namespace ActivityFinder.Server.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public DbProperties DbProperties { get; set; } = new DbProperties();       
        public required string Description { get; set; }
        //public required string Category { get; set; }
        public required DateTime Date { get; set; }

        public int AddressId { get; set; }
        public required Address Address { get; set; }
    }
}
