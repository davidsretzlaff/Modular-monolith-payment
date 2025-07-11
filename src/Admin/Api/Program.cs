using Microsoft.EntityFrameworkCore;
using Admin.Infrastructure;
using Admin.Domain.Repositories;
using Admin.Infrastructure.Repositories;
using Admin.Application.Services;
using Admin.Application.Queries;
using Admin.Application.Integration.Provider;
using Admin.Application.Commands.Companies;
using Admin.Application.Commands.Users;
using Admin.Application.Queries.Companies;
using Admin.Application.Queries.Users;
using Shared.Core.Cqrs;
using Shared.Contracts;
using System.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AdminDbContext>(options =>
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

// Company Command Handlers
builder.Services.AddScoped<ICommandHandler<CreateCompanyCommand, Guid>, CreateCompanyCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateCompanyCommand>, UpdateCompanyCommandHandler>();
builder.Services.AddScoped<ICommandHandler<ActivateCompanyCommand>, ActivateCompanyCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeactivateCompanyCommand>, DeactivateCompanyCommandHandler>();

// User Command Handlers
builder.Services.AddScoped<ICommandHandler<CreateUserCommand, Guid>, CreateUserCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserCommandHandler>();
builder.Services.AddScoped<ICommandHandler<ActivateUserCommand>, ActivateUserCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeactivateUserCommand>, DeactivateUserCommandHandler>();

// Company Query Handlers
builder.Services.AddScoped<IQueryHandler<GetCompanyByIdQuery, CompanyDto?>, GetCompanyByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllCompaniesQuery, IEnumerable<CompanyDto>>, GetAllCompaniesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetActiveCompaniesQuery, IEnumerable<CompanyDto>>, GetActiveCompaniesQueryHandler>();

// User Query Handlers
builder.Services.AddScoped<IQueryHandler<GetUserByIdQuery, UserDto?>, GetUserByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetUsersByCompanyIdQuery, IEnumerable<UserDto>>, GetUsersByCompanyIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllUsersQuery, IEnumerable<UserDto>>, GetAllUsersQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetActiveUsersQuery, IEnumerable<UserDto>>, GetActiveUsersQueryHandler>();

// Legacy Services (to be migrated)
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserService, UserService>();

// Legacy Queries (to be migrated)
builder.Services.AddScoped<ICompanyQueries, CompanyQueries>();
builder.Services.AddScoped<IUserQueries, UserQueries>();

// Repositories
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Integration
builder.Services.AddScoped<IAdminApiProvider, AdminApiProvider>();

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