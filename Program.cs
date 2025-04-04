using DotNetEnv;
using Filters;
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

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<AuthFilter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Blog API", Version = "v1" });

});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API v1"));

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.Run();
