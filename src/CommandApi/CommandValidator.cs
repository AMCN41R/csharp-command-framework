namespace CommandApi
{
    using FluentValidation;

    /// <inheritdoc />
    public abstract class CommandValidator<T> : AbstractValidator<T>
        where T : ICommand
    {
        /// <summary>
        /// Configure the validation rules for this command.
        /// </summary>
        public abstract void ConfigureRules();
    }
}
