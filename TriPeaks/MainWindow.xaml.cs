using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TriPeaks
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Commands

        private void DealCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ChangePlayerExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ResetGameExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ChangeDeckExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private async void ShowHallOfFameExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await Dispatcher.InvokeAsync((Action)(() => {
                (new HallOfFame()).ShowDialog();
            }));
        }

        private async void ShowInfoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await Dispatcher.InvokeAsync((Action)(() =>
            {
                (new AboutDialog()).ShowDialog();
            }));
        }

        #endregion
    }
}
