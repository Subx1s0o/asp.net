using DB.Models;
using Microsoft.EntityFrameworkCore;


namespace DB;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<UserModel> Users { get; set; }

    public DbSet<PostModel> Posts { get; set; }
}
