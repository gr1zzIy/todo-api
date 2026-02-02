using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using todoApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Потрібно для опису ендпоінтів
builder.Services.AddEndpointsApiExplorer();

// OpenAPI (Scalar читає OpenAPI endpoint)
builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // OpenAPI endpoint (в dev можна публікувати)
    app.MapOpenApi();

    // Scalar UI
    app.MapScalarApiReference(options =>
    {
        // опціонально, якщо хочеш інший маршрут UI
        // options.EndpointPathPrefix = "/docs";
    });
}
else
{
    // базова безпека для прод (мінімум)
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Якщо в тебе HTTPS увімкнено
app.UseHttpsRedirection();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();