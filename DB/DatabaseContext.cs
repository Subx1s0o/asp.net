using Microsoft.EntityFrameworkCore;


namespace DB;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Models.Task> Tasks { get; set; }
}
