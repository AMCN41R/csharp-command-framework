namespace CommandApi.Internal.Requests
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the data sent in response to receiving a valid command.
    /// </summary>
    internal class CommandResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResponse"/> class.
        /// </summary>
        /// <param name="command">The name of the command.</param>
        /// <param name="correlationId">The associated correlation identifier.</param>
        /// <param name="executed">Whether or not the command was executed.</param>
        public CommandResponse(
            string command,
            string correlationId,
            bool executed)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (string.IsNullOrWhiteSpace(correlationId))
            {
                throw new ArgumentNullException(nameof(correlationId));
            }

            this.Command = command;
            this.CorrelationId = correlationId;
            this.Executed = executed;
        }

        /// <summary>
        /// Gets the name of the command that was received.
        /// </summary>
        [JsonProperty("command")]
        public string Command { get; }

        /// <summary>
        /// Gets the associated correlation identifier.
        /// </summary>
        [JsonProperty("correlationId")]
        public string CorrelationId { get; }

        /// <summary>
        /// Gets a value indicating whether or not the command was executed or
        /// just validated.
        /// </summary>
        [JsonProperty("executed")]
        public bool Executed { get; }
    }
}
