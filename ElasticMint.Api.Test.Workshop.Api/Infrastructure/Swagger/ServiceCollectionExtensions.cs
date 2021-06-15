using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ElasticMint.Api.Test.Workshop.Api.Infrastructure.Swagger
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(
            this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddSwaggerGen();
        }
    }
}
