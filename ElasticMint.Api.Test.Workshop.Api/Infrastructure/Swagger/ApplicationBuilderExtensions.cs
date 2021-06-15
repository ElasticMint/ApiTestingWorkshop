using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ElasticMint.Api.Test.Workshop.Api.Infrastructure.Swagger
{
    internal static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwagger(
            this IApplicationBuilder app,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider,
            string routePrefix)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(
                    options =>
                    {
                        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }

                        options.RoutePrefix = routePrefix;
                    });
        }
    }
}