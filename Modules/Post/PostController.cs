
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(PostService postService) : Controller
{
    public async Task<IActionResult> FindAll([FromQuery] string page, [FromQuery] string limit)
    {
        return Ok(await postService.FindAll(Convert.ToInt32(page), Convert.ToInt32(limit)));
    }
}