using Microsoft.EntityFrameworkCore;
using TrainingVideoAPI.Data;
using TrainingVideoAPI.Interfaces;
using TrainingVideoAPI.Models;
using TrainingVideoAPI.Repositories;
using TrainingVideoAPI.Services;
using Microsoft.AspNetCore.Http.Features;


var builder = WebApplication.CreateBuilder(args);

// Configure URL and port
builder.WebHost.UseUrls("http://localhost:5050");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular4300",
        policy =>
        {
            policy.WithOrigins("http://localhost:4300")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure PostgreSQL
builder.Services.AddDbContext<VideoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register DI for Repository and Service
builder.Services.AddScoped<IRepository<int, TrainingVideo>, TrainingVideoRepository>();
builder.Services.AddScoped<ITrainingVideoService, TrainingVideoService>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});


var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular4300");

app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
