namespace ContextService.Errors
{
    public class ApiException
    {
        private int StatusCode { get; set; }
        private string Message { get; set; }
        private string details { get; set; }

        public ApiException(int statusCode, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            details = details;
        }
    }
}