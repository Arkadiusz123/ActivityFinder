namespace ActivityFinder.Server.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        //public required string Category { get; set; }
        public required DateTime Date { get; set; }
        public string? OtherInfo { get; set; }

        public DbProperties DbProperties { get; set; } = new DbProperties();

        public int AddressId { get; set; }
        public required Address Address { get; set; }

        public string CreatorId { get; set; }
        public required ApplicationUser Creator { get; set; }

        public List<ApplicationUser> JoinedUsers { get; set; } = [];

    }
}
