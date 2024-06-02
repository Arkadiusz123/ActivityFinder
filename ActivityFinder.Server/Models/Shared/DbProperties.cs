using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityFinder.Server.Models
{
    [ComplexType]
    public class DbProperties
    {
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Edited { get; set; }
        public bool Deleted { get; set; }
    }
}
