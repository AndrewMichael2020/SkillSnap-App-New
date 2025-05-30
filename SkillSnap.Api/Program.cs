using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SkillSnap.Api.Models;
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

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<SkillSnapContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins("https://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseCors("AllowClient");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace SkillSnap.Api
{
    public partial class Program { }
}
