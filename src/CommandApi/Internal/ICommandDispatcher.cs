namespace CommandApi.Internal
{
    using System;
    using System.Threading.Tasks;

    using CommandApi;
    using CommandApi.Internal.Registry;

    /// <summary>
    /// The command dispatcher takes a dynamic command and its handler details,
    /// resolves the handler, auth provider and validator and executes the command.
    /// </summary>
    internal interface ICommandDispatcher
    {
        /// <summary>
        /// Attempts to resolve a handler for the given command and execute it.
        /// </summary>
        /// <typeparam name="T">The command type.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <param name="metadata">The command metadata.</param>
        /// <param name="handlerDetail">The command type information.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DispatchCommand<T>(T command, CommandMetadata metadata, CommandHandlerDetail handlerDetail)
            where T : ICommand;

        /// <summary>
        /// Validates a given command providing that a validator exists.
        /// </summary>
        /// <typeparam name="T">The command type.</typeparam>
        /// <param name="command">The command to validate.</param>
        /// <param name="handlerDetail">The command type information.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method does not need to be called if calling the
        /// <see cref="DispatchCommand{T}(T, CommandMetadata, CommandHandlerDetail)"/>
        /// method as it is called automatically.
        /// </remarks>
        Task ValidateCommand<T>(T command, CommandHandlerDetail handlerDetail)
            where T : ICommand;

        /// <summary>
        /// Authorizes a given command providing that an auth provider exits.
        /// </summary>
        /// <typeparam name="T">The command type.</typeparam>
        /// <param name="command">The command to authorize.</param>
        /// <param name="metadata">The command metadata.</param>
        /// <param name="handlerDetail">The command type information.</param>
        /// <param name="onAuthorizationFailed">
        /// An optional handler that is invoked if authorization fails. If not
        /// provided, an <see cref="UnauthorizedAccessException"/> will be thrown
        /// when authorization fails.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AuthorizeCommand<T>(
            T command,
            CommandMetadata metadata,
            CommandHandlerDetail handlerDetail,
            Func<CommandMetadata, Task>? onAuthorizationFailed = null)
            where T : ICommand;
    }
}
