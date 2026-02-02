using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using todoApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// OpenAPI генерація (потрібна для Scalar)
builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");
app.MapControllers();

// OpenAPI endpoint
app.MapOpenApi();

// Scalar UI
app.MapScalarApiReference();

app.Run();