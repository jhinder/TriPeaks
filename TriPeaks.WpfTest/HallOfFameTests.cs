using System;
using System.Linq;
using Xunit;

namespace TriPeaks.Test
{
    public class HallOfFameTests
    {
        /*
        private HighscoreManager hsm;

        public HallOfFameTests()
        {
            hsm = HighscoreManager.Instance;
        }

        [Fact] 
        public void InsertLargeScore()
        {
            var maxScore = hsm.Highscores.Max(x => x.Score);
            var score = (new Random()).Next(maxScore + 1, maxScore + 500);
            const string entryName = "Unit Test";
            hsm.AddHighscore(entryName, score);
            var newMaxEntry = hsm.Highscores.First();
            Assert.AreEqual(entryName, newMaxEntry.Name);
            Assert.AreEqual(score, newMaxEntry.Score);
            hsm.SaveHighscores(); // for code coverage only
        }

        [Fact]
        public void TryInsertingSmallValue()
        {
            hsm.AddHighscore("Unit Test", int.MinValue);
            Assert.IsFalse(hsm.Highscores.Any(x => x.Score == int.MinValue), "int.MinValue could be inserted.");
        }

        [Fact]
        public void HighAndLowScoreChecker()
        {
            bool canInsertNegative = hsm.IsHighscore(int.MinValue);
            bool canInsertPositive = hsm.IsHighscore(int.MaxValue);
            Assert.IsFalse(canInsertNegative, "int.MinValue could be inserted.");
            Assert.IsTrue(canInsertPositive, "int.MaxValue could not be inserted.");
        }

        [Fact]
        public void AreAnyScoresPresent()
        {
            var any = hsm.LoadHighscores().Any();
            Assert.IsTrue(any, "No high scores could be loaded.");
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = false)]
        public void InsertWithInvalidValues()
        {
            hsm.AddHighscore(null, 12345);
            hsm.AddHighscore(string.Empty, 12345);
        }

        [Fact]
        public void TestHighscoreViewModel()
        {
            var allScores = hsm.Highscores;
            int i = 0;
            foreach (var entry in allScores) {
                Assert.AreEqual(entry.Name, HighscoreViewModel.Names[i]);
                Assert.AreEqual(entry.Score, HighscoreViewModel.Scores[i]);
                i++;
            }
        }
        */
    }
}
