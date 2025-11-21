using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace RiseApi.Helpers.Swagger
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Rise API",
                    Version = "v1",
                    Description = "API para gerenciamento de usuários e planos de desenvolvimento de carreira."
                });

                c.ExampleFilters();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token: Bearer {TOKEN}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddSwaggerExamplesFromAssemblyOf<SwaggerExamples>();
        }
    }
}
