using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ElasticMint.Api.Test.Workshop.Api.Infrastructure.Swagger
{
    internal class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
            _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            options.SchemaGeneratorOptions.SchemaIdSelector = SchemaIdStrategy;
            options.EnableAnnotations();
            options.CustomSchemaIds(x => x.FullName);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
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
                        new List<string>()
                    }
                });

            foreach (var description in _provider.ApiVersionDescriptions)
            {

                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = $"Funds {description.ApiVersion}",
                        Version = description.ApiVersion.ToString()
                    });
            }
        }

        private static string SchemaIdStrategy(Type currentClass)
        {
            var schemaId = currentClass.Name;

            foreach (var customAttributeData in currentClass.CustomAttributes)
            {
                if (customAttributeData.AttributeType.Name.ToLower() != "datacontractattribute")
                {
                    continue;
                }

                UseDataContractAttributeNameIfPresent(customAttributeData);
            }

            return schemaId;

            void UseDataContractAttributeNameIfPresent(CustomAttributeData customAttributeData)
            {
                if (customAttributeData.NamedArguments != null)
                {
                    foreach (var argument in customAttributeData.NamedArguments)
                    {
                        if (argument.MemberName.ToLower() == "name")
                        {
                            schemaId =
                                argument.TypedValue.Value?.ToString()
                                ?? currentClass.Name;
                        }
                    }
                }
            }
        }
    }
}