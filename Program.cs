using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

string connectionString = Environment.GetEnvironmentVariable("DB_URL") ??
    throw new InvalidOperationException("Connection string 'DB_URL' not found.");

builder.Services.AddScoped<TaskService>();

builder.Services.AddDbContext<DB.DatabaseContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.Run();
