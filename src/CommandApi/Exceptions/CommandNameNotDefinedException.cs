namespace CommandApi.Exceptions
{
    using System;

    /// <summary>
    /// An exception that is thrown when trying to resolve a command name from
    /// the <see cref="CommandNameAttribute"/> when the attribute is not present.
    /// </summary>
    [Serializable]
    public class CommandNameNotDefinedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandNameNotDefinedException"/> class.
        /// </summary>
        /// <param name="commandType">The type of command.</param>
        internal CommandNameNotDefinedException(Type commandType)
            : base($"Command name not defined: {commandType.Name}")
        {
        }
    }
}
