namespace Example.AspCore.Commands.Test
{
    using System;
    using System.Threading.Tasks;

    using CommandApi;

    /// <inheritdoc />
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        /// <inheritdoc />
        public Task HandleAsync(TestCommand command, CommandMetadata metadata)
        {
            Console.WriteLine($"TEST COMMAND: {command.Message}");
            return Task.CompletedTask;
        }
    }
}
