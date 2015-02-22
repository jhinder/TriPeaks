using System.Windows;
using System.Windows.Input;
using System.Linq;
using System;

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
    }

    public class HighscoreViewModel
    {
        public string[] Names { get; set; }
        public int[] Scores { get; set; }

        public HighscoreViewModel()
        {
            var preferences = Properties.Settings.Default;

            // The data are stored in the properties Names and Scores
            // (yes, not really sophisticated, but whatever)
            // Model: value1;value2;value3;
            string names = preferences.Names;
            string scores = preferences.Scores;
            const char separator = ';';

            Names = names.Split(separator);
            // What's nice: WPF ignores missing values at indexes, so there's no urgent need to check.

            var scoreArray = scores.Split(separator);
            try {
                Scores = scoreArray.Select(x => Int32.Parse(x)).ToArray();
            }
            catch (FormatException) {
                Scores = new int[10];
            }
            

        }
    }
}
