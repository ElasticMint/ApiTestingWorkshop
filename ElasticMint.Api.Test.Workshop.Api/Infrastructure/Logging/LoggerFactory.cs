using System.Linq;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace ElasticMint.Api.Test.Workshop.Api.Infrastructure.Logging
{
    internal static class LoggerFactory
    {
        public static ILogger Create(IConfiguration configuration, string environment)
        {
            var loggingConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Filter.ByExcluding(x => x.Properties.Any(p => p.Value.ToString().ToLowerInvariant().Contains("health")))
                .WriteTo.Async(x => x.Console());

            var serilogConfiguration = new SerilogConfiguration();
            configuration.GetSection("Serilog").Bind(serilogConfiguration);
            if (!string.IsNullOrWhiteSpace(serilogConfiguration.Seq))
            {
                loggingConfiguration.WriteTo.Seq(serilogConfiguration.Seq);
            }

            return loggingConfiguration.CreateLogger();
        }
    }
}
