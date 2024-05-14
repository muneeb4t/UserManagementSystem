using Azure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using UserManagementSystem.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserManagementSystem
{
    public class CustomResponse
    {
        public int Status { get; set; }
        public object Body { get; set; }
        public string Message { get; set; }
    }
    class ResponseFormattingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseFormattingMiddleware> _logger;
        public ResponseFormattingMiddleware(RequestDelegate next, ILogger<ResponseFormattingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    context.Response.Body = memoryStream;
                    await _next(context);

                    memoryStream.Position = 0;
                    var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                    // Check if the response is a validation error response
                    if (IsValidationErrorResponse(responseBody))
                    {
                        var validationErrors = JsonConvert.DeserializeObject<CustomResponse>(responseBody);
                        var formattedResponse = JsonConvert.SerializeObject(MapToCustomResponse(validationErrors));
                        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(formattedResponse));
                    }
                    else
                    {
                        // Leave other responses unmodified
                        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(responseBody));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while formatting the response.");
                await HandleException(context.Response);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private bool IsValidationErrorResponse(string responseBody)
        {
            // Check if the response contains "One or more validation errors occurred."
            return responseBody.Contains("One or more validation errors occurred.");
        }

        private CustomResponse MapToCustomResponse(CustomResponse validationErrors)
        {
            return new CustomResponse
            {
                Status = validationErrors.Status,
                Body = validationErrors.Body,
                Message = validationErrors.Message
            };
        }

        private async Task HandleException(HttpResponse response)
        {
            response.ContentType = "application/json";
            var errorResponse = JsonConvert.SerializeObject(new CustomResponse
            {
                Status = 500,
                Body = null,
                Message = "Internal Server Error"
            });
            await response.WriteAsync(errorResponse);
        }
    }
}
