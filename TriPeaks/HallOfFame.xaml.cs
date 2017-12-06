using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace TriPeaks
{
    /// <summary>
    /// Interaktionslogik für HallOfFame.xaml
    /// </summary>
    public partial class HallOfFame : Window
    {

        public HallOfFame()
        {
            InitializeComponent();
        }

        private void CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }

    internal class HighscoreViewModel
    {
        
        public static string[] Names
        {
            get { return HighscoreManager.Instance?.Highscores.Select(x => x.Name).ToArray(); }
        }

        public static int[] Scores
        {
            get { return HighscoreManager.Instance?.Highscores.Select(x => x.Score).ToArray(); }
        }

    }

    /// <summary>
    /// The high score manager allows to read and write the high score table.
    /// </summary>
    internal class HighscoreManager
    {
        /// <summary>
        /// The singleton insance of the high score manager.
        /// </summary>
        internal static HighscoreManager Instance { get; } = new HighscoreManager();

        private List<HighScoreEntry> highscores;
        /// <summary>
        /// The list of all high scores.
        /// </summary>
        internal IReadOnlyList<HighScoreEntry> Highscores => highscores;

        private HighscoreManager()
        {
            highscores = LoadHighscores().ToList();
        }

        private bool isDirty = false;

        /// <summary>
        /// Loads the high score table from the disk.
        /// </summary>
        /// <returns>An enumeration of <see cref="HighScoreEntry"/>.</returns>
        internal IEnumerable<HighScoreEntry> LoadHighscores()
        {
            var preferences = Properties.Settings.Default;

            // The data are stored in the properties Names and Scores
            // (yes, not really sophisticated, but whatever)
            // Model: value1;value2;value3;
            string rawNames = preferences.Names;
            string rawScores = preferences.Scores;
            const char separator = ';';

            var names = rawNames.Split(separator);
            // What's nice: WPF ignores missing values at indexes, so there's no urgent need to check.

            var scores = rawScores.Split(separator).Select(x => int.Parse(x)).ToArray();
            for (int i = 0; i < names.Length; i++)
                yield return new HighScoreEntry { Name = names[i], Score = scores[i] };
            
        }

        /// <summary>
        /// Checks if a score is a high score.
        /// </summary>
        /// <param name="score">The score to be checked.</param>
        /// <returns>true if <paramref name="score"/> would be a highscore, otherwise false.</returns>
        internal bool IsHighscore(int score)
        {
             return score >= Highscores.Min(x => x.Score);
        }

        /// <summary>
        /// Adds a new high score to the table.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="score">The score of the player.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null or empty.</exception>
        /// <remarks>If <paramref name="score"/> is less than the lowest score, the call returns without modifying the high score table.</remarks>
        internal void AddHighscore(string name, int score)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (!IsHighscore(score))
                return;

            highscores.Add(new HighScoreEntry { Name = name, Score = score });
            highscores = highscores.OrderByDescending(x => x.Score).Take(10).ToList();
            isDirty = true;
            SaveHighscores();
        }

        /// <summary>
        /// Writes the high score table to the disk.
        /// </summary>
        internal void SaveHighscores()
        {
            if (!isDirty)
                return;
            
            var newNames = Highscores.Select(x => x.Name).ToArray();
            var newScores = Highscores.Select(x => x.Score).ToArray();

            var settings = Properties.Settings.Default;
            settings.Names = string.Join(";", newNames);
            settings.Scores = string.Join(";", newScores);
            settings.Save();

            isDirty = false;
        }

    }

    /// <summary>
    /// An entry in the highscore table.
    /// </summary>
    internal struct HighScoreEntry
    {

        /// <summary>
        /// The name of the player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The score the player achieved.
        /// </summary>
        public int Score { get; set; }
        
    }

}
