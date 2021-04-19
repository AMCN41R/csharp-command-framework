namespace Example.AspCore
{
    using System.Collections.Generic;
    using System.Reflection;

    using CommandApi.DependencyInjection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCommandEndpoint();
                endpoints.MapControllers();
            });
        }
    }
}
