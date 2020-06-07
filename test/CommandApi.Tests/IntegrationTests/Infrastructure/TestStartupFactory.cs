namespace CommandApi.Tests.IntegrationTests.Infrastructure
{
    using System.IO;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Hosting;

    public class TestStartupFactory : WebApplicationFactory<TestStartup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            return base.CreateHost(builder);
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return
                Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(builder =>
                    {
                        builder.UseStartup<TestStartup>();
                    });
        }
    }
}
