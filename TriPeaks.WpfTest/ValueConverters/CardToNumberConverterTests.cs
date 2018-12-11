using System.Linq;
using Xunit;

namespace TriPeaks.WpfTest.ValueConverters
{
    public sealed class CardToNumberConverterTests : ConverterTestsBase<CardToNumberConverter>
    {
        [Fact]
        public void TestConverter()
        {
            var expected = new[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            int i = 0;
            var enumValues = EnumUtils.GetValues<CardValue>().ToArray();
            foreach (var enumval in enumValues)
            {
                string result = converter.Convert(enumval, StringType, null, null) as string;
                Assert.Equal(expected[i++], result);
            }
        }

        [Fact]
        public void TestInvalidValues()
        {
            var ctnNull = converter.Convert(null, StringType, null, null);
            var ctnOOR = converter.Convert((CardValue)100, StringType, null, null);
            Assert.Equal(string.Empty, ctnNull);
            Assert.Equal(string.Empty, ctnOOR);
        }
    }
}
