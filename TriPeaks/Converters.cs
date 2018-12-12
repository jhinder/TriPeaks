using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using TriPeaks.Resources;

namespace TriPeaks
{
    public abstract class ConverterBase
    {
        private protected ConverterBase()
        {
        }

        [ExcludeFromCodeCoverage]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    public sealed class WinLossConverter : ConverterBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            bool didParse = int.TryParse(value.ToString(), out var nValue);
            if (!didParse)
                nValue = 0;
            return string.Format(CultureInfo.CurrentCulture, "{0}${1}", (nValue < 0) ? Strings.LostString : Strings.WonString, Math.Abs(nValue));
        }
    }

    [ValueConversion(typeof(CardColour), typeof(string))]
    public sealed class ColourToSuitConverter : ConverterBase, IValueConverter
    {
        private static readonly IReadOnlyDictionary<CardColour, string> suitMap = new Dictionary<CardColour, string>()
        {
            {CardColour.Club,    "/Resources/suit/club.png" },
            {CardColour.Diamond, "/Resources/suit/diamond.png" },
            {CardColour.Heart,   "/Resources/suit/heart.png" },
            {CardColour.Spade,   "/Resources/suit/spade.png" }
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            bool didGet = suitMap.TryGetValue((CardColour)value, out var imageUri);
            return didGet ? imageUri : string.Empty;
        }
    }

    [ValueConversion(typeof(CardValue), typeof(string))]
    public sealed class CardToNumberConverter : ConverterBase, IValueConverter
    {
        private static readonly IReadOnlyDictionary<CardValue, string> numberMap = new Dictionary<CardValue, string>()
        {
            {CardValue.Ace,   "A"},
            {CardValue.Two,   "2"},
            {CardValue.Three, "3"},
            {CardValue.Four,  "4"},
            {CardValue.Five,  "5"},
            {CardValue.Six,   "6"},
            {CardValue.Seven, "7"},
            {CardValue.Eight, "8"},
            {CardValue.Nine,  "9"},
            {CardValue.Ten,   "10"},
            {CardValue.Jack,  "J"},
            {CardValue.Queen, "Q"},
            {CardValue.King,  "K"}
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            bool didGet = numberMap.TryGetValue((CardValue)value, out var retVal);
            return didGet ? retVal : string.Empty;
        }
    }

    [ValueConversion(typeof(CardColour), typeof(Color))]
    public sealed class SuitToColourConverter : ConverterBase, IValueConverter
    {
        private static readonly IReadOnlyDictionary<CardColour, Color> colourMap = new Dictionary<CardColour, Color>()
        {
            {CardColour.Club,    Colors.Black},
            {CardColour.Diamond, Colors.Red},
            {CardColour.Heart,   Colors.Red},
            {CardColour.Spade,   Colors.Black}
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Colors.Black;
            bool didGet = colourMap.TryGetValue((CardColour)value, out var retColour);
            return didGet ? retColour : Colors.Black;
        }
    }

    /// <summary>
    /// Converts the selected card back index to a border thickness (to show or hide the border)
    /// </summary>
    public class IndexToBorderConverter : IMultiValueConverter
    {
        private static readonly SolidColorBrush blackColour = new SolidColorBrush(Colors.Black);
        private static readonly SolidColorBrush whiteColour = new SolidColorBrush(Colors.White);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            /* First object: selected index (int)
             * Second object: button index (int) */
            if (values == null)
                return whiteColour;
            var castValues = values.Cast<int>();
            if (castValues.Count() < 2)
                return whiteColour;
            return (castValues.ElementAt(0) == castValues.ElementAt(1)) ? blackColour : whiteColour;
        }

        [ExcludeFromCodeCoverage]
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
