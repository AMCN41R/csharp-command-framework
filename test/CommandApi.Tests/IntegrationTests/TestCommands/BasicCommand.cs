namespace CommandApi.Tests.IntegrationTests.TestCommands
{
    using System.Threading.Tasks;

    using CommandApi.Tests.IntegrationTests.Infrastructure;

    using FluentValidation;
    using FluentValidation.Results;

    [CommandName("TEST/BasicCommand")]
    public class BasicCommand : ICommand
    {
        public BasicCommand(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; }

        public string Name { get; }
    }

    public class BasicCommandValidator : CommandValidator<BasicCommand>
    {
        public BasicCommandValidator(ICommandTrackingStore trackingStore)
        {
            this.TrackingStore = trackingStore;
        }

        private ICommandTrackingStore TrackingStore { get; }

        public override void ConfigureRules()
        {
            this.RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than zero.");

            this.RuleFor(x => x.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Name must be provided.");
        }

        public override ValidationResult Validate(ValidationContext<BasicCommand> context)
        {
            this.TrackingStore.Validated(context.InstanceToValidate);

            return base.Validate(context);
        }
    }

    public class BasicCommandAuth : ICommandAuthProvider<BasicCommand>
    {
        public BasicCommandAuth(ICommandTrackingStore trackingStore)
        {
            this.TrackingStore = trackingStore;
        }

        private ICommandTrackingStore TrackingStore { get; }

        public Task<bool> IsAuthorized(BasicCommand command, CommandMetadata metadata)
        {
            this.TrackingStore.Authroized(command, metadata);

            return Task.FromResult(command.Name?.ToLower() != "fail-auth");
        }
    }

    public class BasicCommandHandler : ICommandHandler<BasicCommand>
    {
        public BasicCommandHandler(ICommandTrackingStore trackingStore)
        {
            this.TrackingStore = trackingStore;
        }

        private ICommandTrackingStore TrackingStore { get; }

        public Task HandleAsync(BasicCommand command, CommandMetadata metadata)
        {
            this.TrackingStore.Handled(command, metadata);

            if (command.Name == "handler-exception")
            {
                throw new System.Exception();
            }

            return Task.CompletedTask;
        }
    }
}
