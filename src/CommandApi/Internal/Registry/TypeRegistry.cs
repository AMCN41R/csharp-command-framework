namespace CommandApi.Internal.Registry
{
    using System;

    /// <summary>
    /// The command type registry.
    /// </summary>
    internal class TypeRegistry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRegistry"/> class.
        /// </summary>
        /// <param name="handlerTypes">The registered handler types.</param>
        /// <param name="validatorTypes">The registered validator types.</param>
        /// <param name="authProviderTypes">The registered auth provider types.</param>
        /// <param name="typeDetails">The list of command handler details.</param>
        public TypeRegistry(
            Type[] handlerTypes,
            Type[] validatorTypes,
            Type[] authProviderTypes,
            CommandHandlerDetail[] typeDetails)
        {
            this.HandlerTypes = handlerTypes;
            this.ValidatorTypes = validatorTypes;
            this.AuthProviderTypes = authProviderTypes;
            this.TypeDetails = typeDetails;
        }

        /// <summary>
        /// Gets the list of registered handler types.
        /// </summary>
        public Type[] HandlerTypes { get; }

        /// <summary>
        /// Gets the list of registered validator types.
        /// </summary>
        public Type[] ValidatorTypes { get; }

        /// <summary>
        /// Gets the list of registered auth provider types.
        /// </summary>
        public Type[] AuthProviderTypes { get; }

        /// <summary>
        /// Gets the list of command handler details.
        /// </summary>
        public CommandHandlerDetail[] TypeDetails { get; }
    }
}
