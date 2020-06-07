namespace CommandApi.Tests.IntegrationTests.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using CommandApi.Internal;
    using CommandApi.Internal.Requests;
    using CommandApi.Tests.IntegrationTests.Infrastructure;
    using CommandApi.Tests.IntegrationTests.TestCommands;

    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    using Xunit;

    public class BasicCommandTests :
        IClassFixture<TestStartupFactory>,
        IDisposable
    {
        public BasicCommandTests(TestStartupFactory factory)
        {
            this.Factory = factory;
        }

        private TestStartupFactory Factory { get; }

        public void Dispose()
        {
            var tracker = this.Factory.Services.GetRequiredService<ICommandTrackingStore>();
            tracker.Clear();
        }

        [Fact]
        public async Task TestGet()
        {
            // Arrange
            var client = this.Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/test-get");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact]
        public async Task TestGet_BadRequest()
        {
            // Arrange
            var client = this.Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/bad-test-get");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Throws<HttpRequestException>(
                () => response.EnsureSuccessStatusCode());
        }

        [Fact]
        public async Task CommandPipeline_ValidCommand_IsValidatedAndHandled_AndExpectedResponseIsReturned()
        {
            // Arrange
            var testCommand = new BasicCommand(23, "Michael Jordan");
            var client = this.Factory.CreateClient();

            // Act
            var body = new StringContent(
                JsonConvert.SerializeObject(
                    new { command = "TEST/BasicCommand", body = testCommand }),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/command", body);

            // Assert
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var commandResponse = responseBody.FromJson<CommandResponse>();

            Assert.NotNull(commandResponse);
            Assert.Equal("TEST/BasicCommand", commandResponse.Command);
            Assert.True(commandResponse.Executed);

            var tracker = this.Factory.Services.GetRequiredService<ICommandTrackingStore>();
            var trackedCommand = tracker.Get<BasicCommand>();

            Assert.NotNull(trackedCommand);

            Assert.Equal(typeof(BasicCommand), trackedCommand.CommandType);
            Assert.Equal(testCommand.ToJson(), trackedCommand.CommandJson);

            Assert.True(trackedCommand.Authorized);
            Assert.True(trackedCommand.Validated);
            Assert.True(trackedCommand.Handled);
        }

        [Fact]
        public async Task CommandPipeline_ValidateOnly_IsValidated_AndExtectedResponseIsReturned()
        {
            // Arrange
            var testCommand = new BasicCommand(23, "Michael Jordan");
            var client = this.Factory.CreateClient();

            // Act
            var body = new StringContent(
                JsonConvert.SerializeObject(
                    new { command = "TEST/BasicCommand", body = testCommand }),
                System.Text.Encoding.UTF8,
                "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/command"),
                Headers =
                {
                    { "x-validate-only", "true" },
                },
                Content = body,
            };

            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var commandResponse = responseBody.FromJson<CommandResponse>();

            Assert.NotNull(commandResponse);
            Assert.Equal("TEST/BasicCommand", commandResponse.Command);
            Assert.False(commandResponse.Executed);

            var tracker = this.Factory.Services.GetRequiredService<ICommandTrackingStore>();
            var trackedCommand = tracker.Get<BasicCommand>();

            Assert.NotNull(trackedCommand);

            Assert.Equal(typeof(BasicCommand), trackedCommand.CommandType);
            Assert.Equal(testCommand.ToJson(), trackedCommand.CommandJson);

            Assert.True(trackedCommand.Authorized);
            Assert.True(trackedCommand.Validated);
            Assert.False(trackedCommand.Handled);
        }

        [Fact]
        public async Task CommandPipeline_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var testCommand = new BasicCommand(-1, "Michael Jordan");
            var client = this.Factory.CreateClient();

            // Act
            var body = new StringContent(
                JsonConvert.SerializeObject(
                    new { command = "TEST/BasicCommand", body = testCommand }),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/command", body);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Throws<HttpRequestException>(
                () => response.EnsureSuccessStatusCode());

            var tracker = this.Factory.Services.GetRequiredService<ICommandTrackingStore>();
            var trackedCommand = tracker.Get<BasicCommand>();

            Assert.NotNull(trackedCommand);

            Assert.Equal(typeof(BasicCommand), trackedCommand.CommandType);
            Assert.Equal(testCommand.ToJson(), trackedCommand.CommandJson);

            Assert.True(trackedCommand.Authorized);
            Assert.True(trackedCommand.Validated);
            Assert.False(trackedCommand.Handled);
        }

        [Fact]
        public async Task CommandPipeline_UnknownCommand_ReturnsBadRequest()
        {
            // Arrange
            var testCommand = new { Name = "Michael Jordan" };
            var client = this.Factory.CreateClient();

            // Act
            var body = new StringContent(
                JsonConvert.SerializeObject(
                    new { command = "TEST/UnknownCommand", body = testCommand }),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/command", body);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Throws<HttpRequestException>(
                () => response.EnsureSuccessStatusCode());

            var tracker = this.Factory.Services.GetRequiredService<ICommandTrackingStore>();
            var trackedCommand = tracker.Get<BasicCommand>();

            Assert.Null(trackedCommand);
        }

        [Fact]
        public async Task CommandPipeline_Unauthorised_ReturnsForbidden()
        {
            // Arrange
            var testCommand = new BasicCommand(123, "fail-auth");
            var client = this.Factory.CreateClient();

            // Act
            var body = new StringContent(
                JsonConvert.SerializeObject(
                    new { command = "TEST/BasicCommand", body = testCommand }),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/command", body);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Throws<HttpRequestException>(
                () => response.EnsureSuccessStatusCode());

            var tracker = this.Factory.Services.GetRequiredService<ICommandTrackingStore>();
            var trackedCommand = tracker.Get<BasicCommand>();

            Assert.NotNull(trackedCommand);

            Assert.Equal(typeof(BasicCommand), trackedCommand.CommandType);
            Assert.Equal(testCommand.ToJson(), trackedCommand.CommandJson);

            Assert.True(trackedCommand.Authorized);
            Assert.False(trackedCommand.Validated);
            Assert.False(trackedCommand.Handled);
        }
    }
}
