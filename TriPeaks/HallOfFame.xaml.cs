using System.Windows;
using System.Windows.Input;
using System.Linq;
using System;
using System.Collections.Generic;

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
            this.Close();
        }

        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }

        /// <summary>
        /// Try to write a new entry to the highscores. Will silently fail if the score was too low to insert.
        /// </summary>
        /// <param name="name">The player's name.</param>
        /// <param name="score">The game score.</param>
        public static void InsertSaveEntry(string name, int score)
        {
            string[] names;
            int[] scores;
            HighscoreViewModel.ReadHighscores(out names, out scores);
            if (score < scores.Min())
                return;

            // New score list
            var tmpScores = scores.ToList();
            tmpScores.Add(score);
            var newScores = tmpScores.OrderBy(x => x).Take(10).ToArray();

            // New name list
            var nEntryIndex = tmpScores.IndexOf(score);
            List<string> newNames = new List<string>();
            int i;
            for (i=0; i<nEntryIndex; i++) // The old entries with better players
                newNames.Add(names[i]);
            newNames.Add(name);
            for (i = nEntryIndex + 1; i < 9; i++) // The remaining entries below the new entry
                newNames.Add(names[i]);

            // Write the entries back into the settings file
            string cNames = String.Join(";", newNames.ToArray());
            string cScores = String.Join(";", newScores.ToArray());

            var settings = Properties.Settings.Default;
            settings.Names = cNames;
            settings.Scores = cScores;
            settings.Save();
            
        }

    }

    public class HighscoreViewModel
    {

        private string[] _names;
        /// <summary>
        /// An array of names in the high score list.
        /// </summary>
        public string[] Names
        {
            get { return _names; }
        }

        private int[] _scores;
        /// <summary>
        /// An array of scores in the high score list.
        /// </summary>
        public int[] Scores { get { return _scores; } }

        public HighscoreViewModel()
        {
            ReadHighscores(out _names, out _scores);
        }

        /// <summary>
        /// Reads the high scores from the user's app settings.
        /// </summary>
        /// <param name="names">A <see cref="string"/> array where the names will be inserted into.</param>
        /// <param name="scores">An <see cref="int"/> array where the scores will be inserted into.</param>
        public static void ReadHighscores(out string[] names, out int[] scores)
        {
            var preferences = Properties.Settings.Default;

            // The data are stored in the properties Names and Scores
            // (yes, not really sophisticated, but whatever)
            // Model: value1;value2;value3;
            string rawNames = preferences.Names;
            string rawScores = preferences.Scores;
            const char separator = ';';

            names = rawNames.Split(separator);
            // What's nice: WPF ignores missing values at indexes, so there's no urgent need to check.

            var scoreArray = rawScores.Split(separator);
            try {
                scores = scoreArray.Select(x => Int32.Parse(x)).ToArray();
            }
            catch (FormatException) {
                scores = new int[10];
            }
        }
    }

}
