using System;
using Xunit;

namespace TriPeaks.Test
{
    public class TriPeaksTests
    {
        [Fact]
        public void TestPeakReaching()
        {
            var tpvm = new TriPeaksViewModel();
            int oldWins = tpvm.Wins;

            tpvm.ReachedPeak(1);
            Assert.Contains("Ahmadas", tpvm.AdditionalString);
            Assert.Equal(oldWins + 15, tpvm.Wins);
            oldWins = tpvm.Wins;

            tpvm.ReachedPeak(2);
            Assert.Contains("Gehaldi", tpvm.AdditionalString);
            Assert.Equal(oldWins + 15, tpvm.Wins);
            oldWins = tpvm.Wins;

            tpvm.ReachedPeak(3);
            Assert.Contains("Tri-Conquered", tpvm.AdditionalString);
            Assert.Equal(oldWins + 30, tpvm.Wins);

            tpvm.ReachedPeak(0); // resets the peaks
            tpvm.ReachedPeak(3);
            Assert.Contains("Zackheer", tpvm.AdditionalString);
        }

        [Fact]
        public void TestPeakReachingInvalid()
        {
            var tpvm = new TriPeaksViewModel();
            Assert.Throws<ArgumentOutOfRangeException>(() => tpvm.ReachedPeak(5));
        }

        [Fact]
        public void TestNewGameStart()
        {
            var tpvm = new TriPeaksViewModel();
            tpvm.StartGame(false);
            Assert.True(tpvm.GameInProgress);
            Assert.Equal(0, tpvm.Streak);
            Assert.Equal(0, tpvm.StreakWins);
            Assert.Equal(string.Empty, tpvm.AdditionalString);
            //Assert.NotNull(tpvm.CardManager);
        }

        [Fact]
        public void TestGameRestartWithPenalty()
        {
            var tpvm = new TriPeaksViewModel();
            tpvm.StartGame(false);
            tpvm.ReachedPeak(1);
            int oldScore = tpvm.Score;
            tpvm.Reset();
            Assert.Equal(oldScore - 140, tpvm.Score);
            Assert.Equal(string.Empty, tpvm.AdditionalString);
        }

        [Fact]
        public void TestGameRestartWithoutPenalty()
        {
            var tpvm = new TriPeaksViewModel();
            tpvm.StartGame(false);
            tpvm.Endgame();
            int oldScore = tpvm.Score;
            Assert.True(tpvm.GameInProgress);
            tpvm.StartGame(false);
            Assert.Equal(oldScore, tpvm.Score);
        }
    }
}
