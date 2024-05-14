using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class customUnathorizedResponseMiddleware
{
    private readonly RequestDelegate _next;

    public customUnathorizedResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            // Set response content type to JSON
            context.Response.ContentType = "application/json";

            // Create a custom JSON response
            var jsonResponse = JsonConvert.SerializeObject(new
            {
                status = context.Response.StatusCode,
                body = (object)null,
                message = "Unauthorized"
            });

            // Write the JSON response to the response body
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
