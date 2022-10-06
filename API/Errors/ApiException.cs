namespace API.Errors
{
    public class ApiException
    {
        public ApiException(int statusCode, string? message = null, string? details = null) 
        {
            StatusCode = statusCode;
            if (message!= null) Message = message;
            if (details != null) Details = details;
        }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Details { get; set; }
    }
}