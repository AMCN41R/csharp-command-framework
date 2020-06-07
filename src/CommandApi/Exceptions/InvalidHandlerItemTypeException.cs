namespace CommandApi.Exceptions
{
    using System;

    /// <summary>
    /// Thrown when trying to resolve a command handler that does not implement
    /// the correct interface.
    /// </summary>
    [Serializable]
    public class InvalidHandlerItemTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHandlerItemTypeException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        internal InvalidHandlerItemTypeException(string message)
            : base(message)
        {
        }
    }
}
