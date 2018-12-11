using Xunit;

namespace TriPeaks.WpfTest.ValueConverters
{
    public sealed class WinLossConverterTests : ConverterTestsBase<WinLossConverter>
    {
        [Theory]
        [InlineData(123, "Won $123")]
        [InlineData(-123, "Lost -$123")]
        [InlineData(0, "Won $0")]
        public void TestConverter(int value, string expected)
        {
            var result = converter.Convert(value, StringType, null, null) as string;
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("x", "Won $0")]
        public void TestInvalidValues(string value, string expected)
        {
            var result = converter.Convert(value, StringType, null, null);
            Assert.Equal(expected, result);
        }
    }
}
