using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace TriPeaks
{
    /// <summary>
    /// Interaktionslogik für CardRenderer.xaml
    /// </summary>
    public partial class CardRenderer : UserControl
    {
        internal event EventHandler<CardEventArgs> CardClicked;

        public CardRenderer()
        {
            InitializeComponent();
        }

        private void CardMouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            var card = (DataContext as Card);
            if (card == null || card.Hidden || CardClicked == null)
                return;

            card.Played = true;
            CardClicked(this, new CardEventArgs(card));
        }
    }

    #region Converter

    [ValueConversion(typeof(CardColour), typeof(string))]
    internal class ColourToSuitConverter : IValueConverter
    {
        private static readonly IReadOnlyDictionary<CardColour, string> suitMap = new Dictionary<CardColour, string>()
        {
            {CardColour.Club,    "/Resources/suit/club.png" },
            {CardColour.Diamond, "/Resources/suit/diamond.png" },
            {CardColour.Heart,   "/Resources/suit/heart.png" },
            {CardColour.Spade,   "/Resources/suit/spade.png" }
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            bool didGet = suitMap.TryGetValue((CardColour)value, out var imageUri);
            return didGet ? imageUri : string.Empty;
        }

        [ExcludeFromCodeCoverage]
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(CardValue), typeof(string))]
    internal class CardToNumberConverter : IValueConverter
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

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            bool didGet = numberMap.TryGetValue((CardValue)value, out var retVal);
            return didGet ? retVal : string.Empty;
        }

        [ExcludeFromCodeCoverage]
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(CardColour), typeof(Color))]
    internal class SuitToColourConverter : IValueConverter
    {
        private static readonly IReadOnlyDictionary<CardColour, Color> colourMap = new Dictionary<CardColour, Color>()
        {
            {CardColour.Club,    Colors.Black},
            {CardColour.Diamond, Colors.Red},
            {CardColour.Heart,   Colors.Red},
            {CardColour.Spade,   Colors.Black}
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Colors.Black;
            bool didGet = colourMap.TryGetValue((CardColour)value, out var retColour);
            return didGet ? retColour : Colors.Black;
        }

        [ExcludeFromCodeCoverage]
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region CardEventArgs

    internal class CardEventArgs : RoutedEventArgs
    {
        public Card Card { get; set; }

        public CardEventArgs(Card card)
        {
            Card = card;
        }
    }

    #endregion

}
