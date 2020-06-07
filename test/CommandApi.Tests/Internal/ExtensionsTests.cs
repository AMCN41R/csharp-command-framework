namespace CommandApi.Tests.Internal
{
    using CommandApi.Internal;

    using Xunit;

    public class ExtensionsTests
    {
        #region TryParseBool

        [Theory]
        [InlineData("true", true)]
        [InlineData("True", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("False", false)]
        [InlineData("FALSE", false)]
        public void TryParseBool_ValidBooleanString_ReturnsCorrectBooleanValue(string value, bool expected)
        {
            Assert.Equal(value.TryParseBool(), expected);
        }

        [Theory]
        [InlineData("Not-a-boolean", true)]
        [InlineData("Not-a-boolean", false)]
        [InlineData("123", true)]
        [InlineData("123", false)]
        public void TryParseBool_InvalidBooleanString_ReturnsDefaultValue(string value, bool defaultValue)
        {
            Assert.Equal(value.TryParseBool(defaultValue), defaultValue);
        }

        #endregion
    }
}
