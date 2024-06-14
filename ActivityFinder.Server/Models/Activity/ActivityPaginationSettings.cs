namespace ActivityFinder.Server.Models
{
    public class ActivityPaginationSettings : PaginationSettings
    {
        public string? Address { get; set; }
        public required string State { get; set; }
    }
}
