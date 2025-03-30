
using System.Net;
using Libs.Exceptions;

namespace Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (HttpException httpEx)
        {

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = httpEx.StatusCode;

            var errorResponse = new
            {
                message = httpEx.Message,
                statusCode = httpEx.StatusCode
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new
            {
                message = "An unexpected error occurred",
                details = ex.Message
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
