using Microsoft.AspNetCore.Identity;

namespace ActivityFinder.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Activity> CreatedActivities { get; set; } = [];
        public List<Activity> JoinedActivities { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];
    }
}
