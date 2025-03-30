using Microsoft.EntityFrameworkCore;

namespace Services;

public class TaskService(DB.DatabaseContext db)
{

    public async Task<List<DB.Models.Task>> FindAll()
    {
        return await db.Tasks.ToListAsync();
    }

    public async Task<DB.Models.Task?> FindOne(int id)
    {
        return await db.Tasks.FindAsync(id);
    }

    public async Task<DB.Models.Task> Create(DB.Models.Task task)
    {
        await db.Tasks.AddAsync(task);
        await db.SaveChangesAsync();
        return task;
    }

    public async Task<DB.Models.Task?> Update(DB.Models.Task task)
    {
        var existingTask = await db.Tasks.FindAsync(task.Id);

        if (existingTask is null)
        {
            return null;
        }


        existingTask.Name = task.Name;
        existingTask.Description = task.Description;
        existingTask.IsCompleted = task.IsCompleted;

        try
        {
            await db.SaveChangesAsync();
            return existingTask;
        }
        catch (DbUpdateConcurrencyException)
        {

            return null;
        }
    }

    public async Task<DB.Models.Task?> Delete(int id)
    {
        var task = await db.Tasks.FindAsync(id);

        if (task is null)
        {
            return null;
        }

        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return task;
    }
}
