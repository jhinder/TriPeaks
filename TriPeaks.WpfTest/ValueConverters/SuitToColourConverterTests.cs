using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Xunit;

namespace TriPeaks.WpfTest.ValueConverters
{
    public sealed class SuitToColourConverterTests : ConverterTestsBase<SuitToColourConverter>
    {
        [Fact]
        public void TestConverter()
        {
            var club = converter.Convert(CardColour.Club, ColorType, null, null);
            var diamond = converter.Convert(CardColour.Diamond, ColorType, null, null);
            var heart = converter.Convert(CardColour.Heart, ColorType, null, null);
            var spade = converter.Convert(CardColour.Spade, ColorType, null, null);

            Assert.Equal(Colors.Black, club);
            Assert.Equal(Colors.Red, diamond);
            Assert.Equal(Colors.Red, heart);
            Assert.Equal(Colors.Black, spade);
        }

        [Fact]
        public void TestInvalidValues()
        {
            var stcNull = converter.Convert(null, ColorType, null, null);
            var stcOOR = converter.Convert((CardColour)100, ColorType, null, null);
            Assert.Equal(Colors.Black, stcNull);
            Assert.Equal(Colors.Black, stcOOR);
        }
    }
}
