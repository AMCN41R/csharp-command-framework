# .NET Command API Framework

![CI](https://github.com/AMCN41R/csharp-command-framework/workflows/CI/badge.svg?branch=master)
![Nuget](https://img.shields.io/nuget/v/commandapi)
![GitHub](https://img.shields.io/github/license/amcn41r/csharp-command-framework)

The command framework provides a simple, configurable command processing pipeline that can be added to any .NET project.

It is ideal for CQRS based systems and is designed to be invoked via a single REST endpoint.

The command pipeline is exposed as middleware, but it can easily added to specific endpoint routing using the extension methods within this library. Alternatively, you can just use the underlying framework to create your own pipeline. Check out the [wiki](https://github.com/AMCN41R/csharp-command-framework/wiki) for more information.

## Installing CommandApi
You should install the [CommandApi framework with NuGet](https://www.nuget.org/packages/CommandApi/):

```
Install-Package CommandApi
```

# Basic Usage
To get started you just need to specify a command and its handler, register the dependencies, and add the command api endpoint.

## A simple command
Commands are requests to change the state of the system. They contain the information required to make the necessary changes. They are simple POCOs that *should* be immutable, and **must** implement the `ICommand` interface and use the `CommandNameAttribute`.

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

## Its handler
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

## Register the dependencies
In order to use the middleware, you need to register the required services. This can be done in the `ConfigureServices` method of your `Startup.cs` file:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    // register the command pipeline dependencies
    services.AddCommandBus(
        new List<Assembly>
        {
            // assemblies that contain commands that should be registered
            // ie: if all commands are in the api project...
            typeof(Startup).Assembly
        });
}
```

## Add the route to your API
To add the command pipeline to your api, simply use the `MapCommandEndpoint()` extension method in the `Configure` method of your `Startup.cs` file.

This will add the `"/command"` route to your api. If you want to specify the route, you can use the overload as demonstrated in the example below.

```csharp
app.UseEndpoints(endpoints =>
{
    // add the command endpoint
    // ----------------------------- //
    
    endpoints.MapCommandEndpoint();

    // OR

    endpoints.MapCommandEndpoint("custom/command-route");

    // ----------------------------- //

    endpoints.MapControllers();
});
```

## Invoke the command
To invoke the pipeline, send a POST request to the endpoint you configured above. The endpoint expects a JSON body with two properties...
- **command** - The name specified on the `CommandName` attribute
- **body** - The JSON representation of the command itself

So, using the `AddUser` command above, the request body would be:
```json
{
  "command": "Api/AddUser",
  "body": {
    "id": 1,
    "name": "Jake"
  }
}
```

Using your method of choice, run your API and test it...

> This example uses PowerShell and the `Invoke-WebRequest` cmdlet

```powershell
$body = @{
  command = "Api/AddUser"
  body = @{
    id = 1
    id = "Jake"
  }
} | ConvertTo-Json

Invoke-WebRequest http://localhost:5000/command -Method 'POST' -Body $body
```


# Command Processing
You must specify a command and a handler, but you can optionally provide a validator and authorization provider.

## Command Validation
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

## Command Authorization
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

# The Pipeline
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

Check out the [wiki](https://github.com/AMCN41R/csharp-command-framework/wiki) for more about the available options.

