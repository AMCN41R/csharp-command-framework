namespace CommandApi.Tests.Internal.Requests
{
    using System;

    using CommandApi.Internal.Requests;

    using Xunit;

    public class CommandResponseTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void New_InvalidCommand_Throws(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new CommandResponse(value, "id", true));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void New_InvalidCorrelationId_Throws(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new CommandResponse("name", value, true));
        }

        [Fact]
        public void New_ValidProperties_AreAssignedCorrectly()
        {
            // Arrange / Act
            var sut = new CommandResponse("name", "id", true);

            // Assert
            Assert.Equal("name", sut.Command);
            Assert.Equal("id", sut.CorrelationId);
            Assert.True(sut.Executed);
        }
    }
}
