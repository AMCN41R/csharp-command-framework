namespace CommandApi
{
    using System;

    /// <summary>
    /// Represents information about an incoming command request.
    /// </summary>
    public class CommandRequestInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRequestInfo"/> class.
        /// </summary>
        /// <param name="commandName">The name of the command being handled.</param>
        /// <param name="timestamp">The time the command was received.</param>
        /// <param name="correlationId">The request correlation identifier.</param>
        /// <param name="validateOnly">Whether the command should be validated, or validated and executed.</param>
        public CommandRequestInfo(
            string commandName,
            DateTime timestamp,
            string correlationId,
            bool validateOnly)
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
            this.ValidateOnly = validateOnly;
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
        /// Gets a value indicating whether the command should be validated, or validated and executed.
        /// </summary>
        public bool ValidateOnly { get; }
    }
}
