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
            var ctx = metadata.GetContext<CustomContext>();
            Console.WriteLine($"TEST COMMAND: {command.Message} | User: '{ctx?.Username}'");
            return Task.CompletedTask;
        }
    }
}
