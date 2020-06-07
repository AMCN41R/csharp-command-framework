namespace CommandApi.Internal
{
    using System;
    using System.Threading.Tasks;

    using CommandApi;
    using CommandApi.Exceptions;
    using CommandApi.Internal.Registry;

    using Newtonsoft.Json.Linq;

    /// <inheritdoc />
    internal class CommandBus : ICommandBus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBus"/> class.
        /// </summary>
        /// <param name="commandHandlerRegistry">The command handler registry.</param>
        /// <param name="commandDispatcher">The command dispatcher.</param>
        /// <param name="options">The command bus options.</param>
        public CommandBus(
            ICommandHandlerRegistry commandHandlerRegistry,
            ICommandDispatcher commandDispatcher,
            CommandBusOptions options)
        {
            this.CommandHandlerRegistry = commandHandlerRegistry
                ?? throw new ArgumentNullException(nameof(commandHandlerRegistry));

            this.CommandDispatcher = commandDispatcher
                ?? throw new ArgumentNullException(nameof(commandDispatcher));

            this.Options = options
                ?? throw new ArgumentNullException(nameof(options));
        }

        private ICommandHandlerRegistry CommandHandlerRegistry { get; }

        private ICommandDispatcher CommandDispatcher { get; }

        private CommandBusOptions Options { get; }

        /// <inheritdoc />
        public async Task SendAsync(
            JObject message,
            string commandName,
            CommandMetadata metadata,
            bool validateOnly)
        {
            var handlerDetail = this.CommandHandlerRegistry.FindByName(commandName);

            dynamic command = message.ToObject(handlerDetail.CommandType)
                ?? throw new InvalidCommandException(
                    commandName,
                    handlerDetail.CommandType.Name,
                    "Incoming command was converted to a null object.");

            if (this.Options.AuthorizeBeforeValidate)
            {
                await this.CommandDispatcher.AuthorizeCommand(command, metadata, handlerDetail, this.Options.OnAuthorizationFailed);
                await this.CommandDispatcher.ValidateCommand(command, handlerDetail);
            }
            else
            {
                await this.CommandDispatcher.ValidateCommand(command, handlerDetail);
                await this.CommandDispatcher.AuthorizeCommand(command, metadata, handlerDetail, this.Options.OnAuthorizationFailed);
            }

            if (validateOnly)
            {
                return;
            }

            try
            {
                await this.CommandDispatcher.DispatchCommand(command, metadata, handlerDetail);
            }
            catch (Exception ex)
            {
                if (this.Options.OnHandlerException == null)
                {
                    throw;
                }

                await this.Options.OnHandlerException(ex);
            }
        }
    }
}
