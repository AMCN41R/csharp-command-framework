namespace CommandApi.Exceptions
{
    using System;

    /// <summary>
    /// An exception that is thrown when a command handler's details cannot be resolved
    /// from the <see cref="Internal.Registry.ICommandHandlerRegistry"/>.
    /// </summary>
    [Serializable]
    public class CommandHandlerNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerNotFoundException"/> class.
        /// </summary>
        /// <param name="handlerDescription">A description of the handler that could not be found to include in the exception message.</param>
        internal CommandHandlerNotFoundException(string handlerDescription)
            : base($"Could not find handler command handler | '{handlerDescription}'")
        {
        }
    }
}
