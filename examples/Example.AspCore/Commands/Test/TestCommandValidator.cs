namespace Example.AspCore.Commands.Test
{
    using CommandApi;

    using FluentValidation;

    /// <inheritdoc />
    public class TestCommandValidator : CommandValidator<TestCommand>
    {
        /// <inheritdoc />
        public override void ConfigureRules()
        {
            this.RuleFor(x => x.Message)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("A valid message must be provided.");
        }
    }
}
