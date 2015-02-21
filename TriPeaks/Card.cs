using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriPeaks
{

    /// <summary>
    /// Defines a playing card.
    /// </summary>
    class Card
    {

        /// <summary>
        /// Determines if a card is hidden, i.e. its back is showing.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// The colour of the card, also known as its suit.
        /// </summary>
        public CardColours Colour { get; set; }

        /// <summary>
        /// The value of the card.
        /// </summary>
        public CardValues Value { get; set; }

        /// <summary>
        /// Creates a new card.
        /// </summary>
        /// <param name="colour">The colour (or suit) of the card.</param>
        /// <param name="value">The value of the card.</param>
        /// /// <param name="hidden">Determines if the card is hidden or shown. Default is hidden.</param>
        public Card(CardColours colour, CardValues value, bool hidden = true)
        {
            this.Colour = colour;
            this.Value = value;
            this.Hidden = hidden;
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
    enum CardColours
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
    enum CardValues
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
