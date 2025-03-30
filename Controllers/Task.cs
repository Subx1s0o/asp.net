using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace asp.net.Controllers;


[Route("[controller]")]
[ApiController]
public class TaskController(DB.DbContext context) : Controller
{

    private readonly DB.DbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var tasks = await _context.Tasks.ToListAsync() ?? [];
        return Ok(tasks);
    }
}

