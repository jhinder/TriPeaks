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
            this.Close();
        }

        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }
    }
}
