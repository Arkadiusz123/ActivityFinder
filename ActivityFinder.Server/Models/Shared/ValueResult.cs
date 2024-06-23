namespace ActivityFinder.Server.Models
{
    public class ValueResult<T>
    {
        public ValueResult(T value, bool result, string mesage = "")
        {
            Value = value;
            Success = result;
            Message = mesage;
        }

        public ValueResult(bool result, string mesage = "")
        {
            Success = result;
            Message = mesage;
        }

        public T? Value { get; set; }
        public bool Success { get; set; } = false;
        public string? Message { get; set; }

        public void SetSuccess(T? value)
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

    public class Result
    {
        public Result(bool result, string mesage = "")
        {
            Success = result;
            Message = mesage;
        }

        public bool Success { get; set; } = false;
        public string? Message { get; set; }

        public void SetSuccess()
        {
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
