namespace CommandApi
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using CommandApi.Exceptions;
    using CommandApi.Internal;
    using CommandApi.Internal.Requests;

    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;

    /// <summary>
    /// The command pipeline endpoint middleware.
    /// </summary>
    public class CommandMiddleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMiddleware"/> class.
        /// </summary>
        /// <param name="next">The pipeline request delegate.</param>
        /// <param name="commandBus">The command bus.</param>
        public CommandMiddleware(RequestDelegate next, ICommandBus commandBus)
        {
            // this pipeline is designed to always return a response, therefore
            // the next delegate will never be called
            _ = next;

            this.CommandBus = commandBus
                ?? throw new ArgumentNullException(nameof(commandBus));
        }

        private ICommandBus CommandBus { get; }

        /// <summary>
        /// Invokes the middleware implementation.
        /// </summary>
        /// <param name="context">The current http context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // POST requests only...
            if (context.Request.Method != "POST")
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return;
            }

            using var bodyStream = new System.IO.StreamReader(context.Request.Body);

            var body = await bodyStream.ReadToEndAsync();

            var request = body.FromJson<CommandRequest>();

            if (request == null
                || request.Body == null
                || request.Command == null
                || string.IsNullOrWhiteSpace(request.Command))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            try
            {
                var correlationId = Guid.NewGuid().ToString();

                var validateOnly =
                    context.Request.Headers.ContainsKey(CommandHeaders.ValidateOnly)
                    && context.Request.Headers[CommandHeaders.ValidateOnly].ToString().TryParseBool();

                var metadata = new CommandMetadata(
                    request.Command,
                    DateTime.UtcNow,
                    correlationId);

                await this.CommandBus.SendAsync(
                    request.Body,
                    request.Command,
                    metadata,
                    validateOnly);

                var response = new CommandResponse(
                    request.Command,
                    correlationId,
                    !validateOnly);

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsync(response.ToJson());
                return;
            }
            catch (CommandHandlerNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(
                    new
                    {
                        message = $"Unknown command: '{request.Command}'",
                    }.ToJson());
                return;
            }
            catch (InvalidCommandException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(
                    new
                    {
                        message = $"{request.Command} command is invalid",
                        errors = ex.ValidationErrors,
                    }.ToJson());
                return;
            }
            catch (JsonSerializationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(
                    new
                    {
                        message = $"{request.Command} command is invalid",
                        errors = new List<string> { $"Could not process {ex.Path}. Please check value (and parent) is of correct type." },
                    }.ToJson());
                return;
            }
            catch (UnauthorizedAccessException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
        }
    }
}
