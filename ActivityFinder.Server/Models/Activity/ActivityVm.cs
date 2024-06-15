namespace ActivityFinder.Server.Models
{
    public class ActivityVm
    {
        public required int Id { get; set; }
        public required string Date { get; set; }
        public required string Title { get; set; }
        public required string Address { get; set; }
        public required bool CreatedByUser { get; set; }
    }

    public class ActivityVmWrapper
    {
        public required IEnumerable<ActivityVm> Data { get; set; }
        public required int TotalCount { get; set; }
    }
}
