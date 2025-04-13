
using Microsoft.AspNetCore.Mvc;


namespace Libs.Utils;

public static class Utils
{
    public static IActionResult? GetUserIdFromContext(HttpContext httpContext, out Guid userId)
    {
        userId = Guid.Empty;

        if (!httpContext.Items.TryGetValue("user", out var userObj) || userObj is not Dictionary<string, object> userData)
        {
            return new UnauthorizedObjectResult(new { message = "User not found" });
        }

        if (!userData.TryGetValue("sub", out var userIdObj) ||
            userIdObj is not string userIdString ||
            !Guid.TryParse(userIdString, out userId))
        {
            return new BadRequestObjectResult(new { message = "Invalid user ID" });
        }

        return null;
    }
}

