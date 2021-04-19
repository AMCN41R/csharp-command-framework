namespace CommandApi.Tests
{
    using System;

    using Xunit;

    public class CommandRequestInfoTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void New_ThrowsWhenCommandNameIsInvalid(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new CommandRequestInfo(
                    value,
                    new DateTime(2021, 1, 1),
                    "id",
                    true));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void New_ThrowsWhenCorrelationIdIsInvalid(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new CommandRequestInfo(
                    "name",
                    new DateTime(2021, 1, 1),
                    value,
                    true));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void New_ValidParameteresAreAssignedCorrectly(bool validateOnly)
        {
            // Arrange / Act
            var info = new CommandRequestInfo(
                "name",
                new DateTime(2021, 2, 2),
                "id",
                validateOnly);

            // Assert
            Assert.Equal("name", info.CommandName);
            Assert.Equal(new DateTime(2021, 2, 2), info.Timestamp);
            Assert.Equal("id", info.CorrelationId);
            Assert.Equal(validateOnly, info.ValidateOnly);
        }
    }
}
