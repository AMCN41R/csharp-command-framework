namespace CommandApi.Tests.IntegrationTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Net;

    using CommandApi.DependencyInjection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public class TestStartupWithCommandContext
    {
        public TestStartupWithCommandContext()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // register the command bus
            services.AddCommandBus(
                new List<System.Reflection.Assembly>
                {
                    this.GetType().Assembly,
                },
                opts =>
                {
                    opts.SetCustomContext = (http, info) =>
                    {
                        var headers = http.Request.Headers;

                        string user =
                            headers.ContainsKey("x-user-id")
                                ? headers["x-user-id"].ToString()
                                : null;

                        return new TestCustomContext(user, info.ValidateOnly);
                    };
                });

            // register the helpers
            services.AddSingleton<ICommandTrackingStore, CommandTrackingStore>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/test-get", ctx =>
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.OK;
                    return ctx.Response.WriteAsync("OK");
                });

                endpoints.MapGet("/bad-test-get", ctx =>
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return ctx.Response.WriteAsync("Bad");
                });

                endpoints.MapCommandEndpoint();
            });
        }
    }

    public class TestCustomContext
    {
        public TestCustomContext(
            string userIdHeader,
            bool validateOnlyHeader)
        {
            this.UserIdHeader = userIdHeader;
            this.ValidateOnlyHeader = validateOnlyHeader;
        }

        public string UserIdHeader { get; }

        public bool ValidateOnlyHeader { get; }
    }
}
