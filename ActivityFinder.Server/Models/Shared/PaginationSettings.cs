namespace ActivityFinder.Server.Models
{
    public class PaginationSettings
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public required string SortField { get; set; }
        public bool Asc { get; set; }
    }
}
