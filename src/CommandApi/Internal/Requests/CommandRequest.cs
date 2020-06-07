namespace CommandApi.Internal.Requests
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents a command request object.
    /// </summary>
    internal class CommandRequest
    {
        /// <summary>
        /// Gets or sets the command name.
        /// </summary>
        public string? Command { get; set; }

        /// <summary>
        /// Gets or sets the command body.
        /// </summary>
        public JObject? Body { get; set; }
    }
}
