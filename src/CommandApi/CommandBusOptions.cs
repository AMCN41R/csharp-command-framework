namespace CommandApi
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Options for the command processing pipeline.
    /// </summary>
    public class CommandBusOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether command authorization occurs
        /// before validation. The default value is true.
        /// </summary>
        public bool AuthorizeBeforeValidate { get; set; } = true;

        /// <summary>
        /// Gets or sets an optional delegate that is invoked when an exception
        /// is thrown when executing a command handler. If not provided, the
        /// exception is allowed to bubble up.
        /// </summary>
        public Func<Exception, Task>? OnHandlerException { get; set; }

        /// <summary>
        /// Gets or sets an optional delegate that is invoked if authorization fails.
        /// If not provided, an <see cref="UnauthorizedAccessException"/> will be thrown
        /// when authorization fails.
        /// </summary>
        public Func<CommandMetadata, Task>? OnAuthorizationFailed { get; set; }

        /// <summary>
        /// Gets or sets an optional delegate that is invoked with each web request
        /// to generate a custom context that will be added to the <see cref="CommandMetadata"/>
        /// object.
        /// </summary>
        public Func<HttpContext, CommandRequestInfo, dynamic>? SetCustomContext { get; set; }
    }
}
