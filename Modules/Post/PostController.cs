
using Dto;
using Filters;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[Route("[controller]")]
[ApiController]
public class PostController(PostService postService) : Controller
{
    public async Task<IActionResult> FindAll([FromQuery] string page, [FromQuery] string limit)
    {
        return Ok(await postService.FindAll(Convert.ToInt32(page), Convert.ToInt32(limit)));
    }

    [HttpGet("me")]
    [ServiceFilter(typeof(AuthFilter))]
    public async Task<IActionResult> FindMyPosts([FromQuery] string page, [FromQuery] string limit)
    {
        var errorResult = GetUserIdFromContext(out Guid userId);
        if (errorResult != null)
        {
            return errorResult;
        }

        return Ok(await postService.FindAll(Convert.ToInt32(page), Convert.ToInt32(limit), userId));
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> FindOne(string slug)
    {
        return Ok(await postService.FindOne(slug));
    }

    [HttpPost]
    [ServiceFilter(typeof(AuthFilter))]
    public async Task<IActionResult> Create([FromBody] PostDto user)
    {
        var errorResult = GetUserIdFromContext(out Guid userId);
        if (errorResult != null)
        {
            return errorResult;
        }

        return CreatedAtAction(nameof(Create), await postService.Create(user, userId));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PostDto postDto)
    {
        return Ok(await postService.Update(id, postDto));
    }

    [HttpPut("{id}/like")]
    public async Task<IActionResult> LikePost(Guid id)
    {
        return Ok(await postService.LikePost(id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await postService.Delete(id));
    }


    private IActionResult? GetUserIdFromContext(out Guid userId)
    {
        userId = Guid.Empty;

        if (!HttpContext.Items.TryGetValue("user", out var userObj) || userObj is not Dictionary<string, object> userData)
        {
            return Unauthorized(new { message = "User not found" });
        }

        if (!userData.TryGetValue("sub", out var userIdObj) ||
            userIdObj is not string userIdString ||
            !Guid.TryParse(userIdString, out userId))
        {
            return BadRequest(new { message = "Invalid user ID" });
        }

        return null;
    }


}