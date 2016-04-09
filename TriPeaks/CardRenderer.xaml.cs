using System;
using System.Collections.Generic;
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

        internal delegate void CardClickHandler(object sender, CardEventArgs e);
        internal event CardClickHandler CardClicked;

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

    [ValueConversion(typeof(CardColours), typeof(string))]
    internal class ColourToSuitConverter : IValueConverter
    {

        private static readonly IReadOnlyDictionary<CardColours, string> suitMap = new Dictionary<CardColours, string>()
        {
            {CardColours.Club,    "/Resources/suit/club.png" },
            {CardColours.Diamond, "/Resources/suit/diamond.png" },
            {CardColours.Heart,   "/Resources/suit/heart.png" },
            {CardColours.Spade,   "/Resources/suit/spade.png" }
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            string imageUri;
            bool didGet = suitMap.TryGetValue((CardColours)value, out imageUri);
            return didGet ? imageUri : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(CardValues), typeof(string))]
    internal class CardToNumberConverter : IValueConverter
    {
        private static readonly IReadOnlyDictionary<CardValues, string> numberMap = new Dictionary<CardValues, string>()
        {
            {CardValues.Ace,   "A"},
            {CardValues.Two,   "2"},
            {CardValues.Three, "3"},
            {CardValues.Four,  "4"},
            {CardValues.Five,  "5"},
            {CardValues.Six,   "6"},
            {CardValues.Seven, "7"},
            {CardValues.Eight, "8"},
            {CardValues.Nine,  "9"},
            {CardValues.Ten,   "10"},
            {CardValues.Jack,  "J"},
            {CardValues.Queen, "Q"},
            {CardValues.King,  "K"}
        };
    
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            string retVal;
            bool didGet = numberMap.TryGetValue((CardValues)value, out retVal);
            return didGet ? retVal : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
 	        throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(CardColours), typeof(Color))]
    internal class SuitToColourConverter : IValueConverter
    {

        private static readonly IReadOnlyDictionary<CardColours, Color> colourMap = new Dictionary<CardColours, Color>()
        {
            {CardColours.Club,    Colors.Black},
            {CardColours.Diamond, Colors.Red},
            {CardColours.Heart,   Colors.Red},
            {CardColours.Spade,   Colors.Black}
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Colors.Black;
            Color retColour;
            bool didGet = colourMap.TryGetValue((CardColours)value, out retColour);
            return didGet ? retColour : Colors.Black;
        }

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
