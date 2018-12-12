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
