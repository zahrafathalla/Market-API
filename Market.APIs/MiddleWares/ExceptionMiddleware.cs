
using Market.APIs.Errors;
using System.Net;
using System.Text.Json;

namespace Market.APIs.MiddleWares
{
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next ,ILogger<ExceptionMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            //take an action to request
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                //take an action to response

                //log error
                _logger.LogError(ex.Message);

                //return response as header and body

                //header
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //500
                context.Response.ContentType = "application/json";

                //body
                var response = _environment.IsDevelopment() ? new ApiExeptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                                : new ApiExeptionResponse((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }



        }
    }
}
