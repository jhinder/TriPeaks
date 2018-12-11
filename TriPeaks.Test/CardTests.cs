using Xunit;

namespace TriPeaks.Test
{
    public sealed class CardTests
    {
        [Theory(DisplayName = "Test adjacency of card pairs")]
        [InlineData(CardValue.Ace, CardValue.Two, true)]
        [InlineData(CardValue.Ace, CardValue.King, true)]
        [InlineData(CardValue.Five, CardValue.Eight, false)]
        [InlineData(CardValue.Queen, CardValue.Three, false)]
        [InlineData(CardValue.Ace, CardValue.Ace, false)]
        public void TestAdjacency(CardValue left, CardValue right, bool areAdjacent)
        {
            Assert.Equal(areAdjacent, left.IsAdjacentTo(right));
            Assert.Equal(areAdjacent, right.IsAdjacentTo(left));
        }

        [Fact]
        public void TestCardClass()
        {
            Card c = new Card { Colour = CardColour.Diamond, Hidden = true, Played = false, Value = CardValue.Eight };
            int changes = 0;
            c.PropertyChanged += (s, e) => { changes++; };
            c.Hidden = false;
            c.Played = true;
            Assert.Equal(2, changes);
            Assert.Equal(CardColour.Diamond, c.Colour);
            Assert.False(c.Hidden);
            Assert.True(c.Played);
            Assert.Equal(CardValue.Eight, c.Value);
        }

        /*
        [Fact]
        public void TestCardEvArgs()
        {
            Card c = new Card { Colour = CardColour.Club, Hidden = false, Played = false, Value = CardValue.Ten };
            CardEventArgs ca = new CardEventArgs(c);
            Assert.AreEqual(c, ca.Card);
        }
        */
    }
}
