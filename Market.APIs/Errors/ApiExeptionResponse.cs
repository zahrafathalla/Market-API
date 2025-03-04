﻿namespace Market.APIs.Errors
{
    public class ApiExeptionResponse : ApiResponse
    {
        public string? Details { get; set; }
        public ApiExeptionResponse(int statusCode, string? message = null, string? details = null) 
            : base(500)
        {
            Details = details;
        }
    }
}
