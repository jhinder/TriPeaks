﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TriPeaks
{
    /// <summary>
    /// Interaktionslogik für CardRenderer.xaml
    /// </summary>
    public partial class CardRenderer : UserControl
    {
        public CardRenderer()
        {
            InitializeComponent();
        }
    }

    internal class ColourToSuitConverter : IValueConverter
    {

        private static Dictionary<CardColours, string> suitMap = new Dictionary<CardColours, string>()
        {
            {CardColours.Club,    "/Resources/suit/club.png" },
            {CardColours.Diamond, "/Resources/suit/diamond.png" },
            {CardColours.Heart,   "/Resources/suit/heart.png" },
            {CardColours.Spade,   "/Resources/suit/spade.png" }
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string imageUri;
            suitMap.TryGetValue((CardColours)value, out imageUri);
            return imageUri;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class CardToNumberConverter : IValueConverter
    {
        private static Dictionary<CardValues, string> numberMap = new Dictionary<CardValues, string>()
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
            string retVal;
            numberMap.TryGetValue((CardValues)value, out retVal);
            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
 	        throw new NotImplementedException();
        }
    }

    internal class SuitToColourConverter : IValueConverter
    {

        private static Dictionary<CardColours, Color> colourMap = new Dictionary<CardColours, Color>()
        {
            {CardColours.Club,    Colors.Black},
            {CardColours.Diamond, Colors.Red},
            {CardColours.Heart,   Colors.Red},
            {CardColours.Spade,   Colors.Black}
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color retColour;
            colourMap.TryGetValue((CardColours)value, out retColour);
            return retColour;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}