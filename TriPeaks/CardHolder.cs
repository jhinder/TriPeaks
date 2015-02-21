using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriPeaks
{
    class CardHolder
    {

        private static IList<Card> rawDeck;

        private Stack<Card> bottomStack;

        private Card latestCard;

        static CardHolder()
        {
            // Go through all cards once and add them to the base deck.
            // This needs to be done exactly once per app run -> cctor.
            rawDeck = new List<Card>(GenerateDeck());
        }

        private static void SetHiddenForDeck(bool hidden)
        {
            foreach (Card card in rawDeck)
                card.Hidden = hidden;
        }

        public CardHolder()
        {
            // Place 23 cards in the bottom stack.
            // (Yes, we get 24 cards at first. Hang on a second and you'll see why.)
            int cardCount = 0;
            Random r = new Random();
            Card tmpCard;
            do {
                int rand = r.Next(0, 51);
                tmpCard = rawDeck.ElementAt(rand);
                if (tmpCard.Hidden) {
                    rawDeck.ElementAt(rand).Hidden = false;
                    bottomStack.Push(tmpCard);
                    cardCount++;
                }
            } while (cardCount < 24);

            // Place one card as the initial playing card.
            // For simplicity (no tracking of "burnt" card indexes) we get 24 random cards
            latestCard = bottomStack.Pop();
        }

        private static IEnumerable<Card> GenerateDeck()
        {
            // Generation order: colours, then values
            for (short colour = 0; colour < 4; colour++)
                for (short value = 0; value < 13; value++)
                    yield return new Card((CardColours)colour, (CardValues)value);
        }

    }
}
