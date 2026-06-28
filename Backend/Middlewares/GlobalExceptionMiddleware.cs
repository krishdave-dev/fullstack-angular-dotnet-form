using Backend.Exceptions;
using Backend.Models.DTOs;

namespace Backend.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public  GlobalExceptionMiddleware(RequestDelegate next)
{
    _next = next;
}

public async Task InvokeAsync(
    HttpContext context
)
    {
        try 
        {
            await _next(context);
        }
        
        catch(Exception ex)
        {
            await HandleExceptionAsync(
                context,
                ex
            );
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var response = new ApiResponse<object>();

        switch (ex)
        {
            case NotFoundException:
                context.Response.StatusCode = 404;
                response.StatusCode = 404;
                response.Message = ex.Message;
                break;

            case ValidationException validation:
                context.Response.StatusCode = 400;
                response.StatusCode = 400;
                response.Message = validation.Message;
                response.Errors = validation.Errors;
                break;

            default:
                context.Response.StatusCode = 500;
                response.StatusCode = 500;
                response.Message =
                    "Internal server error";
                break;
        }
    }
}

    