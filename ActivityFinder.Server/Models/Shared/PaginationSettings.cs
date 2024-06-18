using System.ComponentModel.DataAnnotations;

namespace ActivityFinder.Server.Models
{
    public class PaginationSettings
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; }

        [Range(1, 300)]
        public int Size { get; set; }

        public required string SortField { get; set; }
        public bool Asc { get; set; }
    }
}
