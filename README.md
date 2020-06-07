# .NET Command Framework

The command framework provides a simple, configurable command processing pipeline that can be added to any .NET project.

It is ideal for CQRS based systems.

## Commands

Commands are requests to change the state of the system. They contain the information required to make the necessary changes. The must implement the `ICommand` interface and use the `CommandNameAttribute`.

> The command name should be unique within the scope of the running application.

```csharp
[CommandName("Api/AddUser")]
public class AddUser : ICommand
{
    public AddUser(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public int Id { get; }

    public string Name { get; }
}
```

### Command Handlers
Each command must have a handler. Simply implement the generic `ICommandHandler<>` interface.

```csharp
public class AddUserHandler : ICommandHandler<AddUser>
{
    public Task HandleAsync(AddUser command, CommandMetadata metadata)
    {
        // handle the command
    }
}
```

### Command Validation
A command can optionally have a validator. The command will be validated before it is handled.

> The `CommandValidator<>` uses [Fluent Validation](https://fluentvalidation.net/).

```csharp
public class AddUserValidator : CommandValidator<AddUser>
{
    public override void ConfigureRules()
    {
        this.RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than zero.");

        this.RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Name must be provided.");
    }
}
```

### Command Authorization
A command can also optionally have a authorization provider. By default, the command will be authorized before it is validated and handled.

> \* The order of validation and authorization can be changed when registering the pipeline.

```csharp
public class AddUserAuth : ICommandAuthProvider<AddUser>
{
    public Task<bool> IsAuthorized(AddUser command, CommandMetadata metadata)
    {
      // authorization logic
    }
}
```

## The Pipeline

To register the pipeline in your application, you can use the Autofac container builder extension:

> Currently, only Autofac is supported for automatic registration.

You can override some of the default processing options by providing the registration method the options setup action.

```csharp
var builder = new ContainerBuilder();

builder.RegisterCommandBus(
    new List<System.Reflection.Assembly>
    {
        // list the assemblies that contain your commands
    },
    opts =>
    {
        // set options
    });
```

The pipeline has 3 stages:
- Authorization
- Validation
- Execution

By default, when a command is received by the command bus, it is authorized (if an auth provider exists), validated (if a validator exists) and then passed to its handler.

### Processing Order
By default, authorization is performed before validation. This can be switched by setting `AuthorizeBeforeValidate` to `false` in the options setup action.

```csharp
opts =>
{
    opts.AuthorizeBeforeValidate = false;
}
```

### Authorization Failure
By default, if authorization fails, a `System.UnauthorizedAccessException` is thrown. This can be overridden by providing a callback in the options setup action. The callback receives the command metadata as its only argument.

> NOTE: If a callback is provided that does not throw an exception, the processing pipeline **WILL** continue to the next stage.

```csharp
opts =>
{
    opts.OnAuthorizationFailed = metadata =>
    {
        // custom auth failure logic
    };
}
```

### Handler Exceptions
By default, if an exception is thrown by a command handler, the exception is unhandled and allowed to bubble up into your application, allowing you to handle the exception either globally, or inside each handler.

If you want to provide common exception handling inside **ALL** command handlers, you can provide a callback in the options setup action. The callback is passed the exception that was thrown as its only argument.

```csharp
opts =>
{
    opts.OnHandlerException = ex =>
    {
        // custom exception handling
    };
}
```

