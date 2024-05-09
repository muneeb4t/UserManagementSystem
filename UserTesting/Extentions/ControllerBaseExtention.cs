using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Models;

public static class ControllerBaseExtensions
{
    public static IActionResult Ok<T>(this ControllerBase controller, string message = null , T data = default(T))
    {
        return controller.Ok(new ServiceResponse<T>
        {
            Status = 200,
            Message = message,
            Body = data
        });
    }

    public static IActionResult NotFound<T>(this ControllerBase controller, string message = null)
    {
        return controller.NotFound(new ServiceResponse<T>
        {
            Status = 404,
            Message = message ?? "Not Found",
            Body = default(T)
        });
    }
    public static IActionResult BadRequest<T>(this ControllerBase controller, string message = null , T data = default(T))
    {
        return controller.BadRequest(new ServiceResponse<T>
        {
            Status = 400,
            Message = message,
            Body = default(T)
        });
    }
}