using BankApp.Contexts;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using BankApp.Interfaces;
using BankApp.Services;
using BankApp.Repositories;
using BankApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository<int, User>, UserRepository>();
builder.Services.AddScoped<IRepository<int, Account>, AccountRepository>();
builder.Services.AddScoped<IRepository<int, Transaction>, TransactionRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// Also add repositories here as scoped or transient as per your design


builder.Services.AddDbContext<BankingContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
