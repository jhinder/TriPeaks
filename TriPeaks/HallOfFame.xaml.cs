using System;
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

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }
    }

    internal class HighscoreViewModel
    {
        public static string[] Names => HighscoreManager.Instance.GetScoreboard().Select(x => x.Name).ToArray();

        public static int[] Scores => HighscoreManager.Instance.GetScoreboard().Select(x => x.Score).ToArray();
    }
}
