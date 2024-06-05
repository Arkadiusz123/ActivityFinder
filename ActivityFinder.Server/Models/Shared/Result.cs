namespace ActivityFinder.Server.Models
{
    public class Result<T>
    {
        public T? Value { get; set; }
        public bool Success { get; set; } = false;
        public string? Message { get; set; }

        public void SetSuccess(T value)
        {
            Value = value;
            Message = "";
            Success = true;
        }

        public void SetFail(string errorMessage)
        {
            Message = errorMessage;
            Success = false;
        }
    }
}
