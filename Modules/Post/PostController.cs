
using Dto;
using Filters;
using Microsoft.AspNetCore.Mvc;
using Services;
using Libs.Utils;
namespace Controllers;

[Route("[controller]")]
[ApiController]
public class PostController(PostService postService) : Controller
{

    [HttpGet]
    [ServiceFilter(typeof(AuthFilter))]
    public async Task<IActionResult> FindPosts([FromQuery] string page, [FromQuery] string limit)
    {
        var errorResult = Utils.GetUserIdFromContext(HttpContext, out Guid userId);
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
        var errorResult = Utils.GetUserIdFromContext(HttpContext, out Guid userId);
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



}