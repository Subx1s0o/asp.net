using Microsoft.EntityFrameworkCore;
using DbLibContext = Microsoft.EntityFrameworkCore.DbContext;
namespace DB;

public class DbContext(DbContextOptions<DbContext> options) : DbLibContext(options)
{
    public DbSet<Task> Tasks { get; set; }
}
