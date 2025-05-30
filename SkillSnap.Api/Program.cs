using Microsoft.EntityFrameworkCore;
using SkillSnap.Api.Data;
using SkillSnap.Api.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(Program).Assembly);

builder.Services
    .AddDbContext<SkillSnapContext>(options =>
        options.UseSqlite("Data Source=skillsnap.db"))
    .AddAutoMapper(typeof(SkillSnapProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins("https://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowClient");

app.MapControllers();

app.Run();
