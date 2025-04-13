
using Filters;
using Libs.Utils;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(UserService userService) : Controller
{

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int limit = 10)
    {
        return Ok(await userService.FindAll(page, limit));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await userService.FindOne(id.ToString()));
    }

    [HttpGet("me")]
    [ServiceFilter(typeof(AuthFilter))]
    public async Task<IActionResult> GetMe()
    {
        var errorResult = Utils.GetUserIdFromContext(HttpContext, out Guid userId);
        if (errorResult != null) return errorResult;

        return Ok(await userService.FindOne(userId.ToString()));
    }

}
