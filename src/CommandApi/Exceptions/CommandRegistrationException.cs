namespace CommandApi.Exceptions
{
    using System;

    /// <summary>
    /// An exception that is thrown when there is a problem registering commands
    /// and their handlers.
    /// </summary>
    [Serializable]
    public class CommandRegistrationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRegistrationException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        internal CommandRegistrationException(string message)
            : base(message)
        {
        }
    }
}
