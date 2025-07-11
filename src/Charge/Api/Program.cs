using Microsoft.EntityFrameworkCore;
using Charge.Infrastructure;
using Charge.Domain.Repositories;
using Charge.Infrastructure.Repositories;
using Charge.Application.Services;
using Charge.Application.Queries;
using System.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ChargeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Database Connection for Dapper
builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

// Services
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// Queries
builder.Services.AddScoped<ISaleQueries, SaleQueries>();
builder.Services.AddScoped<ITransactionQueries, TransactionQueries>();

// Repositories
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run(); 