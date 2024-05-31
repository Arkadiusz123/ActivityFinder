namespace ActivityFinder.Server.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public required string Address { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public required DateTime Date { get; set; }
    }
}
