namespace CommandApi.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using CommandApi.Internal;
    using CommandApi.Internal.Registry;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/>
    /// to register the command pipeline dependencies.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Creates and registers a command pipeline with the given container
        /// for all commands in the given assemblies.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="commandHandlerAssemblies">The assemblies to scan for commands.</param>
        /// <param name="setupAction">A delegate to configure the options for the command pipeline.</param>
        /// <returns>The service instance.</returns>
        public static IServiceCollection AddCommandBus(
           this IServiceCollection services,
           List<Assembly> commandHandlerAssemblies,
           Action<CommandBusOptions>? setupAction = null)
        {
            var busOptions = new CommandBusOptions();
            setupAction?.Invoke(busOptions);
            services.AddSingleton(busOptions);

            // register a service locator to be used by the command dispatcher
            services.AddSingleton<ServiceFactory>(services =>
            {
                return type => services.GetService(type);
            });

            // register the command bus and dispatcher
            services.AddSingleton<ICommandBus, CommandBus>();
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

            // get the type information for all commands in the given assemblies
            // this will return all of the command handlers, all of the command
            // validators and a list of handler details for the registry
            var types = commandHandlerAssemblies.GetHandlerDetails();

            // register the command handlers, auth providers and validators so
            // they can be resolved dynamically
            types.HandlerTypes.ToList().ForEach(x => services.AddTransient(x));
            types.ValidatorTypes.ToList().ForEach(x => services.AddTransient(x));
            types.AuthProviderTypes.ToList().ForEach(x => services.AddTransient(x));

            // create and register the command handler registry
            services.AddSingleton<ICommandHandlerRegistry>(
                new CommandHandlerRegistry(types.TypeDetails));

            return services;
        }
    }
}
