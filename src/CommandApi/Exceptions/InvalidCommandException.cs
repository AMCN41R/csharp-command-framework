namespace CommandApi.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// An exception that is thrown when a command fails validation, or is considered invalid by its handler.
    /// </summary>
    [Serializable]
    public class InvalidCommandException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandException"/> class.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="error">The error message.</param>
        public InvalidCommandException(
            string commandName,
            string commandType,
            string error)
        {
            this.CommandName = commandName;
            this.CommandType = commandType;

            this.ValidationErrors =
                new ReadOnlyDictionary<string, List<string>>(
                    new Dictionary<string, List<string>>
                    {
                        { "Error", new List<string> { error } },
                    });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandException"/> class.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="validationErrors">The list of validation errors.</param>
        public InvalidCommandException(
            string commandName,
            string commandType,
            IDictionary<string, List<string>> validationErrors)
        {
            this.CommandName = commandName;
            this.CommandType = commandType;
            this.ValidationErrors = new ReadOnlyDictionary<string, List<string>>(validationErrors);
        }

        /// <inheritdoc />
        public override string Message
        {
            get
            {
                var message = $"Command {this.CommandName} of type {this.CommandType} is invalid => ";
                message += string.Join("|", this.ValidationErrors.Select(e => e.Value).Select(g => g));
                return message;
            }
        }

        /// <summary>
        /// Gets the list of validation errors.
        /// </summary>
        public IReadOnlyDictionary<string, List<string>> ValidationErrors { get; }

        private string CommandType { get; }

        private string CommandName { get; }
    }
}
