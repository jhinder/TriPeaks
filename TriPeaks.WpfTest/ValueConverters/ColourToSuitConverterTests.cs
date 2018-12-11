using Xunit;

namespace TriPeaks.WpfTest.ValueConverters
{
    public sealed class ColourToSuitConverterTests : ConverterTestsBase<ColourToSuitConverter>
    {
        [Fact]
        public void TestConverter()
        {
            foreach (var enumVal in EnumUtils.GetValues<CardColour>())
            {
                string result = converter.Convert(enumVal, StringType, null, null) as string;
                Assert.True(result.Contains(enumVal.ToString().ToLower()), "The path didn't contain the name of the enum value.");
            }
        }

        [Fact]
        public void TestInvalidValues()
        {
            var ctsNull = converter.Convert(null, StringType, null, null);
            var ctsOOR = converter.Convert((CardColour)100, StringType, null, null); // OOR = Out of Range
            Assert.Equal(string.Empty, ctsNull);
            Assert.Equal(string.Empty, ctsOOR);
        }
    }
}
