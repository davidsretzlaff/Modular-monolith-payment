using Microsoft.EntityFrameworkCore;
using Catalog.Infrastructure;
using Catalog.Application.Services;
using Catalog.Application.Queries;
using Catalog.Application.Integration;
using Catalog.Application.Integration.Provider;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Repositories;
using Shared.Contracts;
using Catalog.Application.Commands.Plans;
using Catalog.Application.Queries.Plans;
using Shared.Core.Cqrs;
using System.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Database Connection for Dapper
builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

// CQRS Infrastructure
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();

// Command Handlers
builder.Services.AddScoped<ICommandHandler<CreatePlanCommand, Guid>, CreatePlanCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdatePlanCommand>, UpdatePlanCommandHandler>();
builder.Services.AddScoped<ICommandHandler<ActivatePlanCommand>, ActivatePlanCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeactivatePlanCommand>, DeactivatePlanCommandHandler>();

// Query Handlers
builder.Services.AddScoped<IQueryHandler<GetPlanByIdQuery, PlanDto?>, GetPlanByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetPlansByCompanyIdQuery, IEnumerable<PlanDto>>, GetPlansByCompanyIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetActivePlansQuery, IEnumerable<PlanDto>>, GetActivePlansQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllPlansQuery, IEnumerable<PlanDto>>, GetAllPlansQueryHandler>();

// Legacy Services (to be migrated)
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<ICouponService, CouponService>();

// Legacy Queries (to be migrated)
builder.Services.AddScoped<IPlanQueries, PlanQueries>();
builder.Services.AddScoped<ICouponQueries, CouponQueries>();

// Repositories
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();

// Integration
builder.Services.AddScoped<IAdminApiProvider, MockAdminApiProvider>();
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