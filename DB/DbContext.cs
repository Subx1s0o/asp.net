using Microsoft.EntityFrameworkCore;
using DbLibContext = Microsoft.EntityFrameworkCore.DbContext;
namespace DB;

public class DbContext(DbContextOptions<DbContext> options) : DbLibContext(options)
{
    public DbSet<Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Task>().HasKey(t => t.Id);

        modelBuilder.Entity<Task>()
            .Property(t => t.Id)
            .HasDefaultValueSql("gen_random_uuid()");
    }
}
