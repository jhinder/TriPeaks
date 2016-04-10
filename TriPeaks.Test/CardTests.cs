using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TriPeaks.Test
{
    [TestClass]
    public class CardTests
    {

        private CardHolder cardHolder;

        [TestInitialize]
        public void Setup()
        {
            cardHolder = new CardHolder();
        }

        [TestMethod]
        public void TestAdjacency()
        {

            bool adjacentA2 = CardValues.Ace.IsAdjacentTo(CardValues.Two);
            bool adjacentAK = CardValues.Ace.IsAdjacentTo(CardValues.King);
            bool adjacentKA = CardValues.King.IsAdjacentTo(CardValues.Ace);
            bool notAdjacent = CardValues.Five.IsAdjacentTo(CardValues.Eight);
            bool notAdjacentC = CardValues.Queen.IsAdjacentTo(CardValues.Three);
            bool notAdjacentSame = CardValues.Ace.IsAdjacentTo(CardValues.Ace);

            Assert.IsTrue(adjacentA2);
            Assert.IsTrue(adjacentAK);
            Assert.IsTrue(adjacentKA);
            Assert.IsFalse(notAdjacent);
            Assert.IsFalse(notAdjacentC);
            Assert.IsFalse(notAdjacentSame);

        }

        [TestMethod]
        public void TestDeck()
        {
            cardHolder = new CardHolder(); // we must guarantee a frest stack for this test
            Assert.AreEqual(23, cardHolder.BottomStack.Count);
            Assert.AreEqual(23, cardHolder.StackCount);
            Assert.AreEqual(28, cardHolder.PyramidCards.Count);
        }

        [TestMethod]
        public void ShiftEntireStack()
        {
            cardHolder = new CardHolder();
            while (cardHolder.TryMoveStackToCurrent());
            Assert.AreEqual(0, cardHolder.StackCount);
        }

        [TestMethod]
        public void PlayStack()
        {
            int events = 0;
            cardHolder.PropertyChanged += (s, e) => { events++; };
            int oldStackCount = cardHolder.StackCount;
            var oldCard = cardHolder.CurrentCard;
            var didMove = cardHolder.TryMoveStackToCurrent();
            Assert.IsTrue(didMove);
            Assert.AreEqual(3, events); // BottomStack, StackCount, CurrentCard
            int newStackCount = cardHolder.StackCount;
            var newCard = cardHolder.CurrentCard;

            Assert.AreEqual(oldStackCount - 1, newStackCount);
            Assert.AreNotEqual(oldCard, newCard);
        }

        [TestMethod]
        public void PlayEntirePyramid()
        {
            cardHolder = new CardHolder();
            for (int i = 27; i >= 0; i--) {
                cardHolder.PyramidCards[i].Played = true;
                cardHolder.RecalculateCardsTurned();
            }
            Assert.IsTrue(cardHolder.PyramidCards.All(x => x.Played && !x.Hidden));
        }

        [TestMethod]
        public void TestCardClass()
        {
            Card c = new Card { Colour = CardColours.Diamond, Hidden = true, Played = false, Value = CardValues.Eight };
            int changes = 0;
            c.PropertyChanged += (s, e) => { changes++; };
            c.Hidden = false;
            c.Played = true;
            Assert.AreEqual(changes, 2);
            Assert.AreEqual(c.Colour, CardColours.Diamond);
            Assert.AreEqual(c.Hidden, false);
            Assert.AreEqual(c.Played, true);
            Assert.AreEqual(c.Value, CardValues.Eight);
        }
    }
}
