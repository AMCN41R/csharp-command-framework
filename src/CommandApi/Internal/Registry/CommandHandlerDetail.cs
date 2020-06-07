namespace CommandApi.Internal.Registry
{
    using System;

    /// <summary>
    /// This type holds information about a command and its handler. It includes
    /// the commands's type, its name, its handler type and, if it has one, its validator type.
    /// </summary>
    internal class CommandHandlerDetail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerDetail"/> class.
        /// </summary>
        /// <param name="handlerType">The handler type.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="commandName">The command name.</param>
        /// <param name="validatorType">The validator type.</param>
        /// <param name="authProviderType">The auth provider type.</param>
        public CommandHandlerDetail(
            Type handlerType,
            Type commandType,
            string commandName,
            Type? validatorType = null,
            Type? authProviderType = null)
        {
            if (string.IsNullOrWhiteSpace(commandName))
            {
                throw new ArgumentNullException(nameof(commandName));
            }

            this.HandlerType = handlerType;
            this.CommandType = commandType;
            this.CommandName = commandName;
            this.ValidatorType = validatorType;
            this.AuthProviderType = authProviderType;
        }

        /// <summary>
        /// Gets the command handler type.
        /// </summary>
        public Type HandlerType { get; }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public Type CommandType { get; }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string CommandName { get; }

        /// <summary>
        /// Gets the validator type.
        /// </summary>
        public Type? ValidatorType { get; }

        /// <summary>
        /// Gets the auth provider type.
        /// </summary>
        public Type? AuthProviderType { get; }

        /// <summary>
        /// Gets a value indicating whether or not the command has a validator.
        /// </summary>
        public bool HasValidator => this.ValidatorType != null;
    }
}
