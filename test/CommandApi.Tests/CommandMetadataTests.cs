namespace CommandApi.Tests
{
    using System;

    using Xunit;

    public class CommandMetadataTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void New_ThrowsWhenCommandNameIsInvalid(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new CommandMetadata(
                    value,
                    new DateTime(2021, 1, 1),
                    "id"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void New_ThrowsWhenCorrelationIdIsInvalid(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new CommandMetadata(
                    "name",
                    new DateTime(2021, 1, 1),
                    value));
        }

        [Fact]
        public void New_ValidParameteresAreAssignedCorrectly()
        {
            // Arrange / Act
            var info = new CommandMetadata(
                "name",
                new DateTime(2021, 2, 2),
                "id");

            // Assert
            Assert.Equal("name", info.CommandName);
            Assert.Equal(new DateTime(2021, 2, 2), info.Timestamp);
            Assert.Equal("id", info.CorrelationId);
        }

        [Fact]
        public void New_ValidParameteresWithCustomContextAreAssignedCorrectly()
        {
            // Arrange / Act
            var info = new CommandMetadata(
                "name",
                new DateTime(2021, 2, 2),
                "id",
                "test");

            // Assert
            Assert.Equal("name", info.CommandName);
            Assert.Equal(new DateTime(2021, 2, 2), info.Timestamp);
            Assert.Equal("id", info.CorrelationId);

            Assert.NotNull(info.Context);
            Assert.Equal("test", (string)info.Context);
        }
    }
}
