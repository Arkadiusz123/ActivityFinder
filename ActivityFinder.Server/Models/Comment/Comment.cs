namespace ActivityFinder.Server.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public required string Content { get; set; }
        public DbProperties DbProperties { get; set; } = new DbProperties();

        public string UserId { get; set; }
        public required ApplicationUser User { get; set; }

        public int ActivityId { get; set; }
        public required Activity Activity { get; set; }
    }
}
