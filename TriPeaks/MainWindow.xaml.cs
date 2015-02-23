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
            if (viewModel.GameInProgress) {
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
                    viewModel.Reset();
            }
            viewModel.StartGame();

            viewModel.CardManager = new CardHolder();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (viewModel.GameInProgress) {
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

        private void CanDrawFromStack(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (viewModel != null
                            && viewModel.GameInProgress
                            && viewModel.CardManager.StackCount != 0);
        }

        private void StackDrawExecute(object sender, ExecutedRoutedEventArgs e)
        {
            bool moveSuccess = viewModel.CardManager.TryMoveStackToCurrent();
            if (moveSuccess)
                viewModel.Losses += 5;
            else
                viewModel.Endgame();
        }

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

        private int _streak;
        public int Streak
        {
            get { return _streak; }
            set
            {
                _streak = value;
                RaisePropertyChanged("Streak");
            }
        }

        public int StreakWins
        {
            get { return (_streak * (_streak + 1) / 2); }
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

        public bool GameInProgress { get; set; }

        /// <summary>
        /// Finishes a game.
        /// </summary>
        public void Endgame()
        {
            GameInProgress = false;
        }

        public void Reset()
        {
            StartGame();
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        public void StartGame()
        {
            if (GameInProgress)
                Losses += 140;

            reachedPeaks = 0;
            CardManager = new CardHolder();
            GameInProgress = true;
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

    public class CardBackProvider : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Short circuiting is essential here.
            if (value != null
                && !(value is CardHolder) // for the main peaks where we have no binding path
                && (bool)value == false)
                return null; // hidden -> false? No image overlay
            int back = Properties.Settings.Default.Back;
            if (back < 0 || back > 7)
                back = 0;
            return String.Format("/Resources/backs/back_{0}.png", (back + 1));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
