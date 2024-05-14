using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Models;

[Controller]
public abstract class BaseController : ControllerBase
{
    public IActionResult Ok<TMessage, TData>(TMessage message = default(TMessage), TData data = default(TData))
    {
        return Ok(new ServiceResponse<TMessage, TData>
        {
            Status = 200,
            Message = message,
            Body = data
        });
    }

    public IActionResult NotFound<TMessage, TData>(TMessage message = default(TMessage))
    {
        return NotFound(new ServiceResponse<TMessage, TData>
        {
            Status = 404,
            Message = message,
            Body = default(TData)
        });
    }

    public IActionResult BadRequest<TMessage, TData>(TMessage message = default(TMessage), TData data = default(TData))
    {
        return BadRequest(new ServiceResponse<TMessage, TData>
        {
            Status = 400,
            Message = message,
            Body = default(TData)
        });
    }
}
