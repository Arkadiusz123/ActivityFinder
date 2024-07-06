using Microsoft.AspNetCore.Identity;

namespace ActivityFinder.Server.Models
{
    public class ApplicationUser : IdentityUser, IHasDbProperties
    {
        public List<Activity> CreatedActivities { get; set; } = [];
        public List<Activity> JoinedActivities { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];
        public DbProperties DbProperties { get; set; } = new DbProperties();
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
