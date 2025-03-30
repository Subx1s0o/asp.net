using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Middleware;
using Repositories;
using Services;


Env.Load();

var builder = WebApplication.CreateBuilder(args);

string connectionString = Environment.GetEnvironmentVariable("DB_URL") ??
    throw new InvalidOperationException("Connection string 'DB_URL' not found.");

builder.Services.AddDbContext<DB.DatabaseContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.Run();
