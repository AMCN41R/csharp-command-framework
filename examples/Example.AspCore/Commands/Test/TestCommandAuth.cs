namespace Example.AspCore.Commands.Test
{
    using System.Threading.Tasks;

    using CommandApi;

    /// <inheritdoc />
    public class TestCommandAuth : ICommandAuthProvider<TestCommand>
    {
        /// <inheritdoc />
        public Task<bool> IsAuthorized(TestCommand command, CommandMetadata metadata)
        {
            return Task.FromResult(command.Message?.ToLower() != "fail");
        }
    }
}
