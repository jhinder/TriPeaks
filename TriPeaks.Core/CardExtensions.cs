namespace TriPeaks
{
    public static class CardExtensions
    {
        /// <summary>
        /// Determines if two cards are adjacent to each other, according to TriPeaks rules.
        /// </summary>
        /// <param name="value">The first card value</param>
        /// <param name="otherCard">The card value of the other card.</param>
        /// <returns>true if the cards are adjacent, otherwise false.</returns>
        public static bool IsAdjacentTo(this CardValue oneCard, CardValue otherCard)
        {
            // Equal value? Not adjacent.
            if (oneCard == otherCard)
                return false;

            // Kings and aces are adjacent in TriPeaks.
            if ((oneCard == CardValue.Ace && otherCard == CardValue.King)
                || (oneCard == CardValue.King && otherCard == CardValue.Ace))
            {
                return true;
            }

            // Checking for actual adjacency.
            return oneCard == otherCard + 1 || otherCard == oneCard + 1;
        }
    }
}
