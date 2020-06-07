namespace CommandApi.Internal.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using CommandApi;
    using CommandApi.Exceptions;

    using FluentValidation;

    /// <summary>
    /// A set of extensions to register all available command handlers.
    /// </summary>
    internal static class CommandHandlerRegistrationExtensions
    {
        /// <summary>
        /// Gets type information for all commands, handlers and validators in the
        /// given assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to scan.</param>
        /// <returns>Type information on all command, handlers and validators.</returns>
        public static TypeRegistry GetHandlerDetails(this IEnumerable<Assembly> assemblies)
        {
            // get all the concrete types
            var types = assemblies.SelectMany(x => x.ConcreteTypes());

            // Get all types that are command handlers
            var handlers =
                types
                    .Where(t => t.GetInterfaces().Any(IsHandler()))
                    .ToArray();

            if (!handlers.Any())
            {
                throw new CommandRegistrationException($"Could not find any command handlers");
            }

            // Get all types that are command validators
            var validators =
                types
                    .Where(x => x.BaseType.Name == typeof(CommandValidator<>).Name)
                    .ToArray();

            // Get all types that are command auth providers
            var authProviders =
                types
                    .Where(x => x.GetInterfaces().Any(IsAuthProvider()))
                    .ToArray();

            // get the details
            var details =
                handlers
                    .Select(x => CreateHandlerDetail(x, validators, authProviders))
                    .ToArray();

            // ensure no duplicate command names
            var grp = details
                .GroupBy(x => x.CommandName)
                .Where(x => x.Count() > 1)
                .ToList();

            if (grp.Count > 0)
            {
                var msg = string.Join(
                    Environment.NewLine,
                    grp.SelectMany(x => x.Select(y => $"{y.CommandName} | {y.CommandType.Name}")));

                throw new DuplicateCommandNameException(
                    "Command names must be unique"
                    + $"{Environment.NewLine}"
                    + msg);
            }

            return new TypeRegistry(handlers, validators, authProviders, details);
        }

        private static CommandHandlerDetail CreateHandlerDetail(
            Type handlerType,
            IEnumerable<Type> validators,
            IEnumerable<Type> authProviders)
        {
            // get the command handler interface so we can get the command type
            var @interface =
                handlerType
                    .GetInterfaces()
                    .FirstOrDefault(IsHandler());

            if (@interface == null)
            {
                throw new InvalidHandlerItemTypeException($"Command handler type: {handlerType}");
            }

            // get the command type
            var commandType = @interface.GetGenericArguments()[0];

            // get the command name from the CommandNameAttribute
            if (!(commandType
                .GetCustomAttributes(typeof(CommandNameAttribute), false)
                .FirstOrDefault() is CommandNameAttribute attr))
            {
                throw new CommandNameNotDefinedException(commandType);
            }

            // get the validator if there is one
            var validator =
                validators
                    .SingleOrDefault(
                        x => x.BaseType?.GetGenericArguments()[0] == commandType);

            // get the auth provider if there is one
            var authProvider =
                authProviders
                    .SingleOrDefault(
                        x =>
                            x.GetInterfaces()
                             .FirstOrDefault(IsAuthProvider())?
                             .GetGenericArguments()[0] == commandType);

            // return the details
            return new CommandHandlerDetail(handlerType, commandType, attr.Name, validator, authProvider);
        }

        private static Func<Type, bool> IsHandler()
        {
            return x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandHandler<>);
        }

        private static Func<Type, bool> IsAuthProvider()
        {
            return x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandAuthProvider<>);
        }

        private static IEnumerable<Type> ConcreteTypes(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsPublic);
        }
    }
}
