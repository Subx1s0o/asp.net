using Microsoft.AspNetCore.Mvc;
using Services;

namespace asp.net.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController(TaskService taskService) : Controller

    {

        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            var tasks = await taskService.FindAll();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindOne(int id)
        {
            var task = await taskService.FindOne(id);

            if (task is null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DB.Models.Task task)
        {
            var createdTask = await taskService.Create(task);
            return CreatedAtAction(nameof(FindOne), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DB.Models.Task task)
        {
            task.Id = id;
            var updatedTask = await taskService.Update(task);

            if (updatedTask is null)
            {
                return NotFound();
            }

            return Ok(updatedTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await taskService.Delete(id);
            return NoContent();
        }
    }
}
