namespace ActivityFinder.Server.Models
{
    public class ActivityPaginationSettings : PaginationSettings
    {
        public string? Address { get; set; }
        public required string State { get; set; }
        public required ActivityStatus Status { get; set; }
        public required bool Finished { get; set; }
        public required bool Full { get; set; }
    }
}
