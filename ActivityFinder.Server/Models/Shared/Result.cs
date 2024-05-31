namespace ActivityFinder.Server.Models
{
    public class Result<T>
    {
        public T? Value { get; set; }
        public bool Success { get; set; } = false;
        public string? Message { get; set; }
    }
}
