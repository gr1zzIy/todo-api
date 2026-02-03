using System.Reflection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace todoApi.Documentation;

public static class SwaggerExtensions
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        // Реєстрація прикладів з збірки
        services.AddSwaggerExamplesFromAssemblyOf<SwaggerAssemblyMarker>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Todo API",
                Version = "v1",
                Description = "API для управління задачами",
                Contact = new OpenApiContact
                {
                    Name = "Oleksii Ishchenko",
                    Email = "oleksiy26032005@gmail.com"
                },
                License = new OpenApiLicense { Name = "MIT License" }
            });

            options.EnableAnnotations();
            options.ExampleFilters();

            AddXmlComments(options);
            options.OrderActionsBy(api => api.RelativePath);
        });

        return services;
    }

    private static void AddXmlComments(SwaggerGenOptions options)
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);
    }
}