using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriPeaks
{
    class CardHolder : INotifyPropertyChanged
    {
        /// <summary>
        /// The raw deck. Contains all available playing cards.
        /// </summary>
        private static IList<Card> rawDeck;

        private Stack<Card> _bottomStack;
        /// <summary>
        /// The bottom stack from which the player can draw cards,
        /// </summary>
        public Stack<Card> BottomStack {
            get { return _bottomStack; }
            private set
            {
                _bottomStack = value;
                RaisePropertyChanged("BottomStack");
                RaisePropertyChanged("StackCount");
            }
        }

        /// <summary>
        /// Returns how many cards are left in the stack.
        /// </summary>
        public int StackCount
        {
            get
            {
                return BottomStack.Count;
            }
        }

        private Card _currentCard;
        /// <summary>
        /// The current card, on top of which all moves are made.
        /// </summary>
        public Card CurrentCard {
            get { return _currentCard; }
            set
            {
                _currentCard = value;
                _currentCard.Hidden = false;
                RaisePropertyChanged("CurrentCard");
            }
        }

        /// <summary>
        /// A list of all cards in use by the "pyramids".
        /// </summary>
        public List<Card> PyramidCards { get; set; }

        /// <summary>
        /// Static constructor; initiates the raw deck.
        /// </summary>
        static CardHolder()
        {
            // Go through all cards once and add them to the base deck.
            rawDeck = new List<Card>(GenerateDeck());
        }

        /// <summary>
        /// Sets the hidden status of the entire deck.
        /// </summary>
        /// <param name="hidden">Determines if the cards are supposed to be hidden (true) or shown (false).</param>
        private static void SetHiddenForDeck(bool hidden)
        {
            foreach (Card card in rawDeck)
                card.Hidden = hidden;
        }

        /// <summary>
        /// Creates a new card holder/manager.
        /// </summary>
        public CardHolder()
        {
            SetHiddenForDeck(true);

            // Shuffle the deck and place all cards in a queue.
            var shuffledDeck = new Queue<Card>();
            
            Random r = new Random();
            Card tmpCard;
            while (rawDeck.Any(c => c.Hidden))
            {
                int rand = r.Next(0, 52);
                tmpCard = rawDeck.ElementAt(rand);
                if (tmpCard.Hidden) {
                    shuffledDeck.Enqueue(tmpCard);
                    rawDeck.ElementAt(rand).Hidden = false;
                }
            }

            // Place 23 cards in the bottom stack.
            // (Yes, we get 24 cards at first. Hang on a second and you'll see why.)
            BottomStack = new Stack<Card>();
            for (int i = 0; i < 23; i++)
                BottomStack.Push(shuffledDeck.Dequeue());
            foreach (Card c in BottomStack)
                c.Hidden = true;

            // Place one card as the initial playing card.
            CurrentCard = shuffledDeck.Dequeue();
            CurrentCard.Hidden = false;

            // The remaining 28 cards serve as the basis for the pyramid.
            // ... 18 cards of which are hidden, 10 are shown.
            var rehiddenCards = new Queue<Card>();
            for (int i = 0; i < 18; i++) {
                tmpCard = shuffledDeck.Dequeue();
                tmpCard.Hidden = true;
                rehiddenCards.Enqueue(tmpCard);
            }
            foreach (Card card in shuffledDeck)
                rehiddenCards.Enqueue(card);

            // And finally use the correctly turned queue as the basis for the pyramid cards.
            PyramidCards = rehiddenCards.ToList();

        }

        /// <summary>
        /// Looks for any cards that have been opened with the last move and turns them around.
        /// </summary>
        public void RecalculateCardsTurned()
        {
            int n;
            // Step 1: cards 1 to 3 (index 0-2)
            for (n = 0; n < 3; n++)
                PyramidCards[n].Hidden = BothCardsPlayed(2 * (n + 1), 2 * (n + 1) + 1);

            // The 2nd row is not very nice because there is no single function, compared to rows 1 & 3.
            // Maybe we find one some day.
            PyramidCards[3].Hidden = BothCardsPlayed(9, 10);
            PyramidCards[4].Hidden = BothCardsPlayed(10, 11);
            PyramidCards[5].Hidden = BothCardsPlayed(12, 13);
            PyramidCards[6].Hidden = BothCardsPlayed(13, 14);
            PyramidCards[7].Hidden = BothCardsPlayed(15, 16);
            PyramidCards[8].Hidden = BothCardsPlayed(16, 17);

            // Step 3: cards 10 to 18 (index 9 to 17)
            for (n = 9; n < 18; n++)
                PyramidCards[n].Hidden = BothCardsPlayed((n + 9), (n + 10));

        }

        private bool BothCardsPlayed(int index1, int index2)
        {
            return !(PyramidCards[index1].Played && PyramidCards[index2].Played);
        }

        /// <summary>
        /// Moves the top card of the bottom stack to the current playing card.
        /// </summary>
        /// <returns>true if a move was possible and successful, otherwise false.</returns>
        public bool TryMoveStackToCurrent()
        {
            if (BottomStack.Count() == 0)
                return false;

            CurrentCard = BottomStack.Pop();
            CurrentCard.Hidden = false;
            RaisePropertyChanged("CurrentCard");
            RaisePropertyChanged("StackCount");
            return true;
        }

        /// <summary>
        /// Generates a new deck.
        /// </summary>
        /// <returns>An <see cref="IEnumerable"/> of <see cref="TriPeaks.Card"/>.</returns>
        private static IEnumerable<Card> GenerateDeck()
        {
            // Generation order: colours, then values
            for (short colour = 0; colour < 4; colour++)
                for (short value = 0; value < 13; value++)
                    yield return new Card((CardColours)colour, (CardValues)value);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
