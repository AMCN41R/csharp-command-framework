namespace Commands.Tests
{
    using System;

    using CommandApi;

    using Xunit;

    public class CommandMetadataExtensionsTests
    {
        [Fact]
        public void GetContext_NullMetadata_ReturnsNull()
        {
            // Arrange
            CommandMetadata metadata = null;

            // Act
            var result = metadata.GetContext<TestContext1>();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetContext_NullContext_ReturnsNull()
        {
            // Arrange
            var metadata = new CommandMetadata(
                "command",
                new DateTime(2019, 1, 1),
                "id");

            // Act
            var result = metadata.GetContext<TestContext2>();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetContext_WrongType_ReturnsNull()
        {
            // Arrange
            var metadata = new CommandMetadata(
                "command",
                new DateTime(2019, 1, 1),
                "id",
                new TestContext1 { Id = 123 });

            // Act
            var result = metadata.GetContext<TestContext2>();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetContext_CorrectType_ReturnsContextAsType()
        {
            // Arrange
            var metadata = new CommandMetadata(
                "command",
                new DateTime(2019, 1, 1),
                "id",
                new TestContext1 { Id = 123 });

            // Act
            var result = metadata.GetContext<TestContext1>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TestContext1>(result);
            Assert.Equal(123, result.Id);
        }

        private class TestContext1
        {
            public int Id { get; set; }
        }

        private class TestContext2
        {
            public string Name { get; set; }
        }
    }
}
