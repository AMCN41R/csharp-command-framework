namespace CommandApi
{
    using System.Threading.Tasks;

    /// <summary>
    /// The command authorization provider.
    /// </summary>
    /// <typeparam name="T">The command type.</typeparam>
    public interface ICommandAuthProvider<T>
        where T : ICommand
    {
        /// <summary>
        /// Determines if the command can be executed under the current context.
        /// </summary>
        /// <param name="command">The command to authorize.</param>
        /// <param name="metadata">The command metadata.</param>
        /// <returns><c>true</c> if the command is authorized; otherwise, <c>false</c>.</returns>
        Task<bool> IsAuthorized(T command, CommandMetadata metadata);
    }
}
