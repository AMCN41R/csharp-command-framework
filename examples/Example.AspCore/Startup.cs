namespace Example.AspCore
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using CommandApi;
    using CommandApi.DependencyInjection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCommandBus(
                new List<Assembly>
                {
                    typeof(Startup).Assembly,
                },
                opts =>
                {
                    opts.SetCustomContext = (http, info) =>
                    {
                        var user = http.User?.Identity?.Name ?? "<none>";
                        return new Commands.CustomContext(user);
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<TagDescriptionsDocumentFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.EnableAnnotations();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.SwaggerEndpoint("/my-swag.json", "Swaggy");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCommandEndpoint();
                endpoints.MapControllers();
            });
        }
    }

    public class Test
    {
        public string Desc { get; set; }
    }

    public class TagDescriptionsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<OpenApiTag>
            {
                new OpenApiTag { Name = "Products", Description = "Browse/manage the product catalog" },
                new OpenApiTag { Name = "Orders", Description = "Submit orders" },
            };

            var commandType = typeof(Commands.Test.TestCommand);

            // get the command name from the CommandNameAttribute
            if (!(commandType
                .GetCustomAttributes(typeof(CommandNameAttribute), false)
                .FirstOrDefault() is CommandNameAttribute attr))
            {
                throw new Exception();
            }

            var props = commandType
                .GetProperties()
                .Select(p => new { Name = p.Name, Schema = new OpenApiSchema { Type = p.PropertyType.Name } })
                .ToDictionary(k => k.Name, v => v.Schema);

            HashSet<string> ppp = commandType
                .GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() != null)
                .Select(x => x.Name)
                .ToHashSet();

            var sss = new OpenApiSchema
            {
                Description = "testing",
                Type = "object",
                Properties = props,
                Required = ppp,
            };

            swaggerDoc.Components.Schemas.Add(
                attr.Name,
                sss);

            swaggerDoc.Paths.Add("comTest", new OpenApiPathItem
            {
                Description = "the desc",
                Operations = new Dictionary<OperationType, OpenApiOperation>
                {
                    {
                        OperationType.Post,
                        new OpenApiOperation
                        {
                            Tags = new List<OpenApiTag>{ new OpenApiTag { Name = "Home"} },
                            Summary = "op summary",
                            Description = "op desc",
                            RequestBody = new OpenApiRequestBody
                            {
                                Description = "req desc",
                                Content = new Dictionary<string, OpenApiMediaType>
                                {
                                    {
                                        "application/json",
                                        new OpenApiMediaType { Schema = sss }
                                    },
                                },
                            },
                        }
                    },
                },
                Summary = "the summary",
            });
        }
    }
}
