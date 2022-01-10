namespace _0_Framework.Application
{
    public class OperationResult
    {
        public bool IsSucceeded { get; set; }
        public string? Message { get; set; }

        public OperationResult Succeeded(string message = "Operation has been successfully proceeded")
        {
            Message = message;
            IsSucceeded = true;
            return this;
        }

        public OperationResult Failed(string message)
        {
            Message = message;
            IsSucceeded = false;
            return this;
        }

    }
}