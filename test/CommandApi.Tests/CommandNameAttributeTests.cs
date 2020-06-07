namespace Commands.Tests
{
    using CommandApi;

    using Xunit;

    public class CommandNameAttributeTests
    {
        [Fact]
        public void New_NameOnly_AssignsPropertyValuesCorrectly()
        {
            // Arrange / Act
            var sut = new CommandNameAttribute("command/name");

            // Assert
            Assert.Equal("command/name", sut.Name);
        }
    }
}
