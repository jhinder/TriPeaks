using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TriPeaks.Test
{
    [TestClass]
    public class TriPeaksTests
    {

        [TestMethod]
        public void TestPeakReaching()
        {
            var tpvm = new TriPeaksViewModel();
            int oldWins = tpvm.Wins;

            tpvm.ReachedPeak(1);
            Assert.IsTrue(tpvm.AdditionalString.Contains("Ahmadas"));
            Assert.AreEqual(oldWins + 15, tpvm.Wins);
            oldWins = tpvm.Wins;

            tpvm.ReachedPeak(2);
            Assert.IsTrue(tpvm.AdditionalString.Contains("Gehaldi"));
            Assert.AreEqual(oldWins + 15, tpvm.Wins);
            oldWins = tpvm.Wins;

            tpvm.ReachedPeak(3);
            Assert.IsTrue(tpvm.AdditionalString.Contains("Tri-Conquered"));
            Assert.AreEqual(oldWins + 30, tpvm.Wins);

            tpvm.ReachedPeak(0); // resets the peaks
            tpvm.ReachedPeak(3);
            Assert.IsTrue(tpvm.AdditionalString.Contains("Zackheer"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes = false)]
        public void TestPeakReachingInvalid()
        {
            var tpvm = new TriPeaksViewModel();
            tpvm.ReachedPeak(5);
        }

        [TestMethod]
        public void TestNewGameStart()
        {
            var tpvm = new TriPeaksViewModel();
            tpvm.StartGame(false);
            Assert.IsTrue(tpvm.GameInProgress);
            Assert.AreEqual(0, tpvm.Streak);
            Assert.AreEqual(0, tpvm.StreakWins);
            Assert.AreEqual(string.Empty, tpvm.AdditionalString);
            Assert.IsNotNull(tpvm.CardManager);
        }

        [TestMethod]
        public void TestGameRestartWithPenalty()
        {
            var tpvm = new TriPeaksViewModel();
            tpvm.StartGame(false);
            tpvm.ReachedPeak(1);
            int oldScore = tpvm.Score;
            tpvm.Reset();
            Assert.AreEqual(oldScore - 140, tpvm.Score);
            Assert.AreEqual(string.Empty, tpvm.AdditionalString);
        }

        [TestMethod]
        public void TestGameRestartWithoutPenalty()
        {
            var tpvm = new TriPeaksViewModel();
            tpvm.StartGame(false);
            tpvm.Endgame();
            int oldScore = tpvm.Score;
            Assert.IsFalse(tpvm.GameInProgress);
            tpvm.StartGame(false);
            Assert.AreEqual(oldScore, tpvm.Score);
        }
    }
}
