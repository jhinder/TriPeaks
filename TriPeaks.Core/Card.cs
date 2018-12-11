using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TriPeaks
{
    /// <summary>
    /// Defines a playing card.
    /// </summary>
    public sealed class Card : INotifyPropertyChanged
    {
        private bool _hidden = false;
        private bool _played = false;

        /// <summary>
        /// Determines if a card is hidden, i.e. its back is showing.
        /// </summary>
        public bool Hidden
        {
            get { return _hidden; }
            set
            {
                _hidden = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the colour of the card.
        /// </summary>
        /// <value>The colour, or suit, of the card.</value>
        public CardColour Colour { get; set; }

        /// <summary>
        /// Gets or sets the value of the card.
        /// </summary>
        public CardValue Value { get; set; }

        /// <summary>
        /// Gets or sets if this card has already been played.
        /// </summary>
        /// <remarks>
        /// This property is only used for the main playing area, i.e. the three peaks.
        /// It is ignored everywhere else.
        /// </remarks>
        public bool Played
        {
            get { return _played; }
            set
            {
                _played = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public sealed class CardPlayedEventArgs : EventArgs
    {
        internal CardPlayedEventArgs(CardValue value, CardColour colour, bool wasPeak)
        {
            Value = value;
            Colour = colour;
            WasPeakCard = wasPeak;
        }

        public CardValue Value { get; }

        public CardColour Colour { get; }

        public bool WasPeakCard { get; }
    }
}
