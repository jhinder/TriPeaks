using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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

        private TriPeaksViewModel viewModel;
        internal static bool GameInSession { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            viewModel = this.DataContext as TriPeaksViewModel;
        }

        #region Commands

        private void DealCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Redeal();
        }

        private async void ChangePlayerExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await AsyncDialog<PlayerNameDialog>();
        }

        private void ResetGameExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var mres = MessageBox.Show("Do you really want to reset your score to zero?",
                "TriPeaks Reset",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.Yes);
            if (mres == MessageBoxResult.Yes)
                ; // Reset statistics
        }

        private async void ChangeDeckExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await AsyncDialog<BackSelectDialog>();
        }

        private async void ShowHallOfFameExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await AsyncDialog<HallOfFame>();
        }

        private async void ShowInfoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await AsyncDialog<AboutDialog>();
        }

        private async Task AsyncDialog<W>()
            where W : Window, new()
        {
            await Dispatcher.InvokeAsync((Action)(() =>
            {
                (new W()).ShowDialog();
            }));
        }

        #endregion

        private void Redeal()
        {
            if (GameInSession) {
                // Dealing while game in session = $140 penalty
                var mres = MessageBox.Show("You are trying to deal with cards left in play for a penalty of 140 dollars. "
                    + "Do you really want to redeal?",
                    "TriPeaks Penalty",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.Yes);
                if (mres == MessageBoxResult.No)
                    return;
                else
                    ; // -$140
            }
            GameInSession = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (GameInSession) {
                var mres = MessageBox.Show("You are exiting with cards left in play for a penalty of 140 dollars."
                    + " - OUCH!! Do you really want to quit?",
                    "Exiting TiPreaks",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.Yes);

                if (mres == MessageBoxResult.No)
                    e.Cancel = true;
                else
                    ; // -$140
            }
        }

        private void WindowCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }

    internal class TriPeaksViewModel : INotifyPropertyChanged
    {

        private bool _showStatistics;
        public bool ShowStatistics {
            get { return _showStatistics; }
            set
            {
                _showStatistics = value;
                RaisePropertyChanged("ShowStatistics");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class BoolToVisibilityConverter : MarkupExtension, IValueConverter
    {

        private static BoolToVisibilityConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
 	        if (_converter == null)
                _converter = new BoolToVisibilityConverter();
            return _converter;
        }

        public BoolToVisibilityConverter() { }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
