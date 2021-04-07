namespace Example.AspCore.Commands.Test
{
    using System.ComponentModel.DataAnnotations;

    using CommandApi;

    /// <summary>
    /// A command for testing the pipeline.
    /// </summary>
    [CommandName("TEST/TEST_COMMAND")]
    public class TestCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestCommand"/> class.
        /// </summary>
        /// <param name="message">A test message.</param>
        public TestCommand(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the test message.
        /// </summary>
        [Required]
        public string Message { get; }

        public int Id { get; set; }
    }
}
