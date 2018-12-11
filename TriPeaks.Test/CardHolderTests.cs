using System.Linq;
using Xunit;

namespace TriPeaks.Test
{
    public class CardHolderTests
    {
        private readonly CardHolder cardHolder;

        public CardHolderTests()
        {
            cardHolder = new CardHolder();
        }

        [Fact(DisplayName = "Test the state of a fresh deck")]
        public void TestDeck()
        {
            Assert.Equal(23, cardHolder.BottomStack.Count);
            Assert.Equal(23, cardHolder.StackCount);
            Assert.Equal(28, cardHolder.PyramidCards.Count);
        }

        [Fact(DisplayName = "Test the state of a used deck")]
        public void ShiftEntireStack()
        {
            while (cardHolder.TryMoveStackToCurrent())
            {
                // Use up the entire stack
            }
            Assert.Equal(0, cardHolder.StackCount);
        }

        [Fact]
        public void PlayStack()
        {
            int events = 0;
            cardHolder.PropertyChanged += (s, e) => { events++; };
            int oldStackCount = cardHolder.StackCount;
            var oldCard = cardHolder.CurrentCard;
            var didMove = cardHolder.TryMoveStackToCurrent();
            Assert.True(didMove);
            Assert.Equal(3, events); // BottomStack, StackCount, CurrentCard
            int newStackCount = cardHolder.StackCount;
            var newCard = cardHolder.CurrentCard;

            Assert.Equal(oldStackCount - 1, newStackCount);
            Assert.NotEqual(oldCard, newCard);
        }

        [Fact]
        public void PlayEntirePyramid()
        {
            for (int i = 27; i >= 0; i--) {
                cardHolder.PyramidCards[i].Played = true;
                cardHolder.RecalculateCardsTurned();
            }
            Assert.True(cardHolder.PyramidCards.All(x => x.Played && !x.Hidden));
        }
    }
}
