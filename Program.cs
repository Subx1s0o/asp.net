using DotNetEnv;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);


string connectionString = Environment.GetEnvironmentVariable("DB_URL") ??
    throw new InvalidOperationException("Connection string 'DB_URL' not found.");

builder.Services.AddDbContext<DB.DbContext>(options =>
    options.UseNpgsql(connectionString)
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging());

builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();


app.Run();
