namespace CommandApi.DependencyInjection
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;

    /// <summary>
    /// Provides extension methods for <see cref="IEndpointRouteBuilder"/>
    /// to add a command pipeline.
    /// </summary>
    public static class CommandsEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Adds a command pipeline endpoint to the <see cref="IEndpointRouteBuilder"/>
        /// with the template "/command".
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add command endpoint to.</param>
        /// <returns>A convention routes for the command endpoint.</returns>
        public static IEndpointConventionBuilder MapCommandEndpoint(
            this IEndpointRouteBuilder endpoints)
        {
            return endpoints.MapCommandEndpoint("/command");
        }

        /// <summary>
        /// Adds a command pipeline endpoint to the <see cref="IEndpointRouteBuilder"/>
        /// with the specified template.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add command endpoint to.</param>
        /// <param name="pattern">The URL pattern of the command endpoint.</param>
        /// <returns>A convention routes for the command endpoint.</returns>
        public static IEndpointConventionBuilder MapCommandEndpoint(
            this IEndpointRouteBuilder endpoints,
            string pattern)
        {
            var pipeline = endpoints
                .CreateApplicationBuilder()
                .UseMiddleware<CommandMiddleware>()
                .Build();

            return endpoints
                .MapPost(pattern, pipeline)
                .WithDisplayName("Command Pipeline");
        }
    }
}
