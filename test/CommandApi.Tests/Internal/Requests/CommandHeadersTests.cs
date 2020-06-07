namespace CommandApi.Tests.Internal.Requests
{
    using CommandApi.Internal.Requests;

    using Xunit;

    public class CommandHeadersTests
    {
        [Theory]
        [InlineData(CommandHeaders.ValidateOnly, "x-validate-only")]
        public void CommandHeaders_ActualValue_IsTheExpectedValue(string actual, string expected)
        {
            Assert.Equal(expected, actual);
        }
    }
}
