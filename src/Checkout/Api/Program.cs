using Microsoft.EntityFrameworkCore;
using Checkout.Infrastructure;
using Checkout.Application.Services;
using Checkout.Application.Queries;
using Checkout.Domain.Repositories;
using Checkout.Infrastructure.Repositories;
using Shared.Contracts;
using Catalog.Application.Integration.Provider;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure;
using Catalog.Application.Queries.Plans;
using Shared.Core.Cqrs;
using System.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database - Checkout
builder.Services.AddDbContext<CheckoutDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Database - Catalog (for internal communication)
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Database Connection for Dapper
builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

// Services
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Queries
builder.Services.AddScoped<ICustomerQueries, CustomerQueries>();

// Repositories
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IPaymentGateway, FakePaymentGateway>();

// CQRS Infrastructure (for Catalog integration)
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

// Catalog Query Handlers (for internal communication) 
builder.Services.AddScoped<IQueryHandler<GetPlanByIdQuery, Catalog.Application.Dtos.PlanDto?>, GetPlanByIdQueryHandler>();

// Catalog repositories (for internal communication)
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();

// Integration - Direct internal communication with Catalog module
builder.Services.AddScoped<ICatalogApiProvider, CatalogApiProvider>();

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