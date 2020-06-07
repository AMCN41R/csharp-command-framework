namespace CommandApi.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CommandApi;
    using CommandApi.Exceptions;
    using CommandApi.Internal.Registry;

    /// <inheritdoc />
    internal class CommandDispatcher : ICommandDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDispatcher"/> class.
        /// </summary>
        /// <param name="serviceLocator">A delegate for resolving service instances by type.</param>
        public CommandDispatcher(ServiceFactory serviceLocator)
        {
            this.Resolve = serviceLocator;
        }

        private ServiceFactory Resolve { get; }

        /// <inheritdoc />
        public async Task DispatchCommand<T>(T command, CommandMetadata metadata, CommandHandlerDetail handlerDetail)
            where T : ICommand
        {
            var commandHandler = (ICommandHandler<T>)this.Resolve(handlerDetail.HandlerType);

            await commandHandler.HandleAsync(command, metadata);
        }

        /// <inheritdoc />
        public Task ValidateCommand<T>(T command, CommandHandlerDetail handlerDetail)
            where T : ICommand
        {
            // do we have a validator?
            if (handlerDetail?.ValidatorType == null || !handlerDetail.HasValidator)
            {
                // no, so return
                return Task.CompletedTask;
            }

            var validator = (CommandValidator<T>)this.Resolve(handlerDetail.ValidatorType);

            validator.ConfigureRules();
            var result = validator.Validate(command);

            if (result.IsValid)
            {
                return Task.CompletedTask;
            }

            var validationErrorsByProperty = result.Errors.GroupBy(e => e.PropertyName);
            var validationErrors = new Dictionary<string, List<string>>();

            foreach (var propertyErrors in validationErrorsByProperty)
            {
                validationErrors.Add(propertyErrors.Key, propertyErrors.Select(e => e.ErrorMessage).ToList());
            }

            throw new InvalidCommandException(
                handlerDetail.CommandName,
                handlerDetail.CommandType.ToString(),
                validationErrors);
        }

        /// <inheritdoc />
        public async Task AuthorizeCommand<T>(
            T command,
            CommandMetadata metadata,
            CommandHandlerDetail handlerDetail,
            Func<CommandMetadata, Task>? onAuthorizationFailed)
            where T : ICommand
        {
            if (handlerDetail.AuthProviderType == null)
            {
                return;
            }

            var authProvider = (ICommandAuthProvider<T>)this.Resolve(handlerDetail.AuthProviderType);

            if (!await authProvider.IsAuthorized(command, metadata))
            {
                if (onAuthorizationFailed == null)
                {
                    throw new UnauthorizedAccessException(
                            "You do not have permission to execute this command.");
                }

                await onAuthorizationFailed(metadata);
            }
        }
    }
}
