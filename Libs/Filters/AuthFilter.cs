using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services;

namespace Filters;

public class AuthFilter(JwtService jwtService) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var token = httpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ")?.Last();

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "No token provided" });
            return;
        }

        try
        {
            var payload = jwtService.ValidateToken(token);

            if (payload == null || !payload.TryGetValue("exp", out var expValue))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Invalid token payload" });
                return;
            }

            var exp = Convert.ToInt64(expValue);
            var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (exp < currentUnixTime)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Token expired" });
                return;
            }

            httpContext.Items["user"] = payload;
        }
        catch (Exception)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Invalid or expired token" });
        }
    }


    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}
