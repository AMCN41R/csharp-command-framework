namespace CommandApi
{
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The command bus takes a dynamic command and attempts to resolve type
    /// information about the command, its handler and validator from a command
    /// registry. It then sens the raw command and its handler information
    /// onto the command dispatcher.
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// Send a command onto the bus to be validated and executed.
        /// </summary>
        /// <param name="command">The raw command object.</param>
        /// <param name="commandName">The command name.</param>
        /// <param name="metadata">The command metadata.</param>
        /// <param name="validateOnly">Whether the command should be validated, or validated and executed.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// Thrown when the request is not authorized by the authorization provider.
        /// </exception>
        /// <exception cref="CommandApi.Exceptions.InvalidCommandException">
        /// Thrown when the command fails validation is considered invalid.
        /// </exception>
        Task SendAsync(
            JObject command,
            string commandName,
            CommandMetadata metadata,
            bool validateOnly = false);
    }
}
