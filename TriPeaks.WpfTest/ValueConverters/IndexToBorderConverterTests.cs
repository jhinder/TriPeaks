using System;
using System.Windows.Media;
using Xunit;

namespace TriPeaks.WpfTest.ValueConverters
{
    public sealed class IndexToBorderConverterTests
    {
        private static readonly Type ColorType = typeof(Color);
        private readonly IndexToBorderConverter converter;

        public IndexToBorderConverterTests()
        {
            converter = new IndexToBorderConverter();
        }

        [Fact]
        public void TestConverter()
        {
            var itbEquals = converter.Convert(new object[] { 1, 1 }, ColorType, null, null) as SolidColorBrush;
            Assert.Equal(Colors.Black, itbEquals.Color);
            var itbNotEquals = converter.Convert(new object[] { 1, 2 }, ColorType, null, null) as SolidColorBrush;
            Assert.Equal(Colors.White, itbNotEquals.Color);
            var itbExtraElements = converter.Convert(new object[] { 1, 1, 2 }, ColorType, null, null) as SolidColorBrush;
            Assert.Equal(Colors.Black, itbExtraElements.Color);
        }

        [Fact]
        public void TestInvalidValues()
        {
            var itbNull = converter.Convert(null, ColorType, null, null) as SolidColorBrush;
            var itbTooShort = converter.Convert(new object[] { 0 }, ColorType, null, null) as SolidColorBrush;
            var itbZeroArray = converter.Convert(new object[] { null, null }, ColorType, null, null) as SolidColorBrush;
            Assert.Equal(Colors.White, itbNull.Color);
            Assert.Equal(Colors.White, itbTooShort.Color);
            Assert.Equal(Colors.White, itbZeroArray.Color);
        }
    }
}
