using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
                    viewModel.Losses += 140;
            }
            GameInSession = true;

            viewModel.CardManager = new CardHolder();
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
                    viewModel.Losses += 140;
            }
        }

        private void WindowCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        #region Debug functions

        private void btnSetString(object sender, RoutedEventArgs e)
        {
            viewModel.AdditionalString = "This is a string!";
        }

        private void btnDrawFromStack(object sender, RoutedEventArgs e)
        {
            if (viewModel.CardManager == null)
                return; // will not happen once everything is in place
            bool moveSuccess = viewModel.CardManager.TryMoveStackToCurrent();
            if (moveSuccess)
                viewModel.Losses += 5;
            else
                MessageBox.Show("No cards left");
        }

        private void btnPeak1(object sender, RoutedEventArgs e)
        {
            viewModel.ReachedPeak(1);
        }

        private void btnPeak2(object sender, RoutedEventArgs e)
        {
            viewModel.ReachedPeak(2);
        }

        private void btnPeak3(object sender, RoutedEventArgs e)
        {
            viewModel.ReachedPeak(3);
        }

        private void btnResetPeaks(object sender, RoutedEventArgs e)
        {
            viewModel.ReachedPeak(0);
        }

        #endregion

    }

    #region View Model

    internal class TriPeaksViewModel : INotifyPropertyChanged
    {

        private string _additionalString;
        public string AdditionalString
        {
            get { return _additionalString;  }
            set
            {
                _additionalString = value;
                RaisePropertyChanged("AdditionalString");
            }
        }

        private int _wins;
        public int Wins {
            get { return _wins;  }
            set
            {
                _wins = value;
                RaisePropertyChanged("Wins");
                RaisePropertyChanged("Score");
            }
        }

        private int _losses;
        public int Losses {
            get { return _losses; }
            set
            {
                _losses = value;
                RaisePropertyChanged("Losses");
                RaisePropertyChanged("Score");
            }
        }

        public int Score
        {
            get { return Wins + (Losses * (-1)); }
        }

        private CardHolder _cardHolder;
        public CardHolder CardManager
        {
            get { return _cardHolder; }
            set
            {
                _cardHolder = value;
                RaisePropertyChanged("CardManager");
            }
        }

        private string[] peakNames = { "Ahmadas", "Gehaldi", "Zackheer" };
        private short reachedPeaks;

        public void ReachedPeak(short number)
        {
            if (number == 0) {
                reachedPeaks = 0;
                return;
            }
            if (number < 1 || number > 3)
                throw new ArgumentOutOfRangeException();
            reachedPeaks++;
            int bonus = (reachedPeaks != 3 ? 15 : 30);
            Wins += bonus;
            AdditionalString = String.Format("You have {0} giving you a ${1} bonus!",
                (reachedPeaks == 3 ? "Tri-Conquered" : String.Format("reached Peak {0}", peakNames[number - 1])),
                bonus);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    #endregion

    #region Converters

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

    internal class WinLossConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int nValue = Int32.Parse(value.ToString());
            return String.Format("{0}${1}", (nValue < 0) ? "Lost -" : "Won ", Math.Abs(nValue));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

}
