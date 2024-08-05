
namespace Market.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Unauthorized, you are not allowed",
                403 => "Forbidden, you do not have permission",
                404 => "Not found, the resource could not be located",
                500 => "Internal server error, something went wrong",
                502 => "Bad gateway, received an invalid response",
                503 => "Service unavailable, please try again later",
                504 => "Gateway timeout, the server took too long to respond",
                _ => "An unknown error occurred"
            };

        }
    }


}
