
namespace DB.Models;

public class Task
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public bool IsCompleted { get; set; } = false;
}
