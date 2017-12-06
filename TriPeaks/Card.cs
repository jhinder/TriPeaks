using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TriPeaks
{

    /// <summary>
    /// Defines a playing card.
    /// </summary>
    class Card : INotifyPropertyChanged
    {

        private bool _hidden = false;
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
        /// The colour of the card, also known as its suit.
        /// </summary>
        public CardColours Colour { get; set; }

        /// <summary>
        /// The value of the card.
        /// </summary>
        public CardValues Value { get; set; }

        private bool _played = false;
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
        
        /// <summary>
        /// Creates a new card.
        /// </summary>
        public Card() { }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    static class CardExtensions
    {
        /// <summary>
        /// Determines if two cards are adjacent to each other, according to TriPeaks rules.
        /// </summary>
        /// <param name="value">The first card value</param>
        /// <param name="otherCard">The card value of the other card.</param>
        /// <returns>true if the cards are adjacent, otherwise false.</returns>
        public static bool IsAdjacentTo(this CardValues oneCard, CardValues otherCard)
        {
            // Equal value? Not adjacent.
            if (oneCard == otherCard)
                return false;

            // Kings and aces are adjacent in TriPeaks.
            if ((oneCard == CardValues.Ace && otherCard == CardValues.King)
                || (oneCard == CardValues.King && otherCard == CardValues.Ace))
                return true;

            // Checking for actual adjacency.
            short a = (short)oneCard;
            short b = (short)otherCard;
            return (a == b + 1 || b == a + 1);
        }

    }

    /// <summary>
    /// Defines all possible card colours.
    /// </summary>
    internal enum CardColours
    {
        /// <summary>Club (♣, black)</summary>
        Club,
        /// <summary>Diamond (♦, red)</summary>
        Diamond,
        /// <summary>Heart (♥, red)</summary>
        Heart,
        /// <summary>Spade (♠, black)</summary>
        Spade
    }

    /// <summary>
    /// Defines all possible card values.
    /// </summary>
    internal enum CardValues
    {
        /// <summary>Ace (1)</summary>
        Ace,
        /// <summary>Two (2)</summary>
        Two,
        /// <summary>Three (3)</summary>
        Three,
        /// <summary>Four (4)</summary>
        Four,
        /// <summary>Five (5)</summary>
        Five,
        /// <summary>Six (6)</summary>
        Six,
        /// <summary>Seven (7)</summary>
        Seven,
        /// <summary>Eight (8)</summary>
        Eight,
        /// <summary>Nine (9)</summary>
        Nine,
        /// <summary>Ten (10)</summary>
        Ten,
        /// <summary>Jack (11)</summary>
        Jack,
        /// <summary>Queen (12)</summary>
        Queen,
        /// <summary>King (13)</summary>
        King
    }

}
