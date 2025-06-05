using ocuNotify.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using ocuNotify.Interfaces;
using ocuNotify.Services;
using ocuNotify.Models;
using ocuNotify.Repositories;
using ocuNotify.Misc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register your services
builder.Services.AddScoped<ITokenService, TokenService>();

// Explicitly specify your own IAuthenticationService interface to avoid ambiguity
builder.Services.AddScoped<ocuNotify.Interfaces.IAuthenticationService, GoogleAuthenticationService>();

builder.Services.AddScoped<IRepository<string, User>, UserRepository>();
builder.Services.AddScoped<IRepository<int, UploadedFile>, UploadedFileRepository>();

// DB Connection with PostgreSQL
builder.Services.AddDbContext<NotifyContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// SignalR
#region CORS
builder.Services.AddCors(options=>{
    options.AddDefaultPolicy(policy=>{
        policy.WithOrigins("http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
#endregion
builder.Services.AddSignalR();

// Configure Authentication BEFORE building the app
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = ClaimTypes.Role,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"] ?? string.Empty))
        };
    });

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(); 
// IMPORTANT: Use authentication before authorization
app.UseAuthentication();
app.UseAuthorization();

// SignalR hubs
app.MapHub<NotifyHub>("/notifyHub");

// API Controllers
app.MapControllers();

app.Run();
