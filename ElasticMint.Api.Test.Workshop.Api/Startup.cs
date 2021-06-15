using ElasticMint.Api.Test.Workshop.Api.Infrastructure.Routing;
using ElasticMint.Api.Test.Workshop.Api.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace ElasticMint.Api.Test.Workshop.Api
{
    public class Startup
    {
        public const string RoutePrefix = "api";

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(
                    options =>
                    {
                        options.EnableEndpointRouting = false;
                        options.Conventions.Insert(0, new RouteConvention($"{RoutePrefix}/v{{version:apiVersion}}"));
                    });

            services
                .AddApiVersioning()
                .AddVersionedApiExplorer()
                .AddSwagger()
                .AddHealthChecks();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseSerilogRequestLogging(
                    options =>
                        options.GetLevel =
                        (
                            _,
                            _,
                            _) => LogEventLevel.Verbose)
                .UseRouting()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); })
                .UseSwagger(apiVersionDescriptionProvider, RoutePrefix);
        }
    }
}
