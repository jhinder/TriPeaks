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

        /// <summary>
        /// The current card, on top of which all moves are made.
        /// </summary>
        public Card CurrentCard { get; set; }

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
        /// Moves the top card of the bottom stack to the current playing card.
        /// </summary>
        /// <returns>true if a move was possible and successful, otherwise false.</returns>
        public bool TryMoveStackToCurrent()
        {
            if (BottomStack.Count() == 0)
                return false;

            CurrentCard = BottomStack.Pop();
            RaisePropertyChanged("BottomCard");
            RaisePropertyChanged("StackCount");
            return true;
        }

        /// <summary>
        /// Tries to move a card from the pyramids to the current card.
        /// </summary>
        /// <param name="index">The index of the card within the list.</param>
        /// <returns>Returns if the move was successful or allowed.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when the index is not between 0 and 27.</exception>
        public bool MovePyramidCardToCurrent(int index)
        {
            if (index < 0 || index >= 27)
                throw new ArgumentOutOfRangeException("Pyramid index must be between 0 and 27");

            var peekCard = PyramidCards.ElementAt(index);
            if (peekCard.Hidden)
                return false;
            if (!CurrentCard.Value.IsAdjacentTo(peekCard.Value))
                return false;

            // It's okay to move the card
            CurrentCard = peekCard;

            //PyramidCards.ElementAt(index)
            // TODO hide from view
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
