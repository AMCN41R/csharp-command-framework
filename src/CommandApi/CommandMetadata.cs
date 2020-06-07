namespace CommandApi
{
    using System;

    /// <summary>
    /// An object that holds additional information that may be useful to a
    /// command handler.
    /// </summary>
    public class CommandMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMetadata"/> class.
        /// </summary>
        /// <param name="commandName">The name of the command being handled.</param>
        /// <param name="timestamp">The time the command was received.</param>
        /// <param name="correlationId">The request correlation identifier.</param>
        /// <param name="context">A custom context object.</param>
        public CommandMetadata(
            string commandName,
            DateTime timestamp,
            string correlationId,
            dynamic? context = null)
        {
            if (string.IsNullOrWhiteSpace(commandName))
            {
                throw new ArgumentNullException(nameof(commandName));
            }

            if (string.IsNullOrWhiteSpace(correlationId))
            {
                throw new ArgumentNullException(nameof(correlationId));
            }

            this.CommandName = commandName;
            this.Timestamp = timestamp;
            this.CorrelationId = correlationId;
            this.Context = context;
        }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string CommandName { get; }

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the correlation identifier.
        /// </summary>
        public string CorrelationId { get; }

        /// <summary>
        /// Gets the custom context object.
        /// </summary>
        public dynamic? Context { get; }
    }
}
