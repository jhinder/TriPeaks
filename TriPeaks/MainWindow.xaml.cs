using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace TriPeaks
{
    using System.Globalization;
    using Resources;

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private TriPeaksViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = DataContext as TriPeaksViewModel;
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
            await Dispatcher.InvokeAsync(() => {
                (new W()).ShowDialog();
            });
        }

        #endregion

        private void Redeal()
        {
            if (viewModel.GameInProgress) {
                // Dealing while game in session = $140 penalty
                var mres = MessageBox.Show(Strings.RedealPenalty, Strings.RedealPenaltyTitle,
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
                var mres = MessageBox.Show(Strings.ExitingPenalty, Strings.ExitingTitle,
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
            Close();
        }

        private void CanDrawFromStack(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (viewModel != null
                            && viewModel.GameInProgress
                            && viewModel.CardManager.StackCount != 0);
        }

        private void StackDrawExecute(object sender, ExecutedRoutedEventArgs e)
        {
            bool moveSuccess = viewModel.CardManager.TryMoveStackToCurrent();
            if (moveSuccess) {
                viewModel.Losses += 5;
                viewModel.Streak = 0;
                viewModel.AdditionalString = string.Empty;
            } else {
                viewModel.Endgame();
            }
        }

        private void PeakCardClicked(object sender, CardEventArgs e)
        {
            if (!viewModel.CardManager.CurrentCard.Value.IsAdjacentTo(e.Card.Value))
                return;
            // The cards match!
            (sender as CardRenderer).Visibility = Visibility.Collapsed;
            viewModel.CardManager.CurrentCard = e.Card;
            viewModel.Streak++;
            viewModel.Wins += viewModel.Streak; // 1 card = $1, 2 cards = $2, ...
            viewModel.CardManager.RecalculateCardsTurned();
        }

    }

    #region View Model

    internal class TriPeaksViewModel : INotifyPropertyChanged
    {

        private string _additionalString = string.Empty;
        public string AdditionalString
        {
            get { return _additionalString;  }
            set
            {
                _additionalString = value;
                RaisePropertyChanged();
            }
        }

        private int _wins;
        public int Wins {
            get { return _wins;  }
            set
            {
                _wins = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Score));
            }
        }

        private int _losses;
        public int Losses {
            get { return _losses; }
            set
            {
                _losses = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Score));
            }
        }

        private int _streak;
        public int Streak
        {
            get { return _streak; }
            set
            {
                _streak = value;
                RaisePropertyChanged();
            }
        }

        public int StreakWins
        {
            get { return (_streak * (_streak + 1) / 2); }
        }

        public int Score
        {
            get { return Wins - Losses; }
        }

        private CardHolder _cardHolder;
        public CardHolder CardManager
        {
            get { return _cardHolder; }
            set
            {
                _cardHolder = value;
                RaisePropertyChanged();
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
            StartGame(throughReset: true);
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        public void StartGame(bool throughReset = false)
        {
            if (throughReset && GameInProgress)
                Losses += 140;

            reachedPeaks = 0;
            CardManager = new CardHolder();
            GameInProgress = true;
            Streak = 0;
            AdditionalString = string.Empty;
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
                throw new ArgumentOutOfRangeException(nameof(number), number, "The peak number must be between 1 and 3.");
            reachedPeaks++;
            int bonus = (reachedPeaks != 3 ? 15 : 30);
            Wins += bonus;
            AdditionalString = string.Format(CultureInfo.CurrentCulture, Strings.PeakConqueredInfo,
                (reachedPeaks == 3 ? Strings.TriConquered : string.Format(CultureInfo.CurrentCulture, Strings.ReachedPeakN, peakNames[number - 1])),
                bonus);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    #endregion

    #region Converters

    [ValueConversion(typeof(int), typeof(string))]
    internal class WinLossConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            int nValue;
            bool didParse = int.TryParse(value.ToString(), out nValue);
            if (!didParse)
                nValue = 0;
            return string.Format(CultureInfo.CurrentCulture, "{0}${1}", (nValue < 0) ? Strings.LostString : Strings.WonString, Math.Abs(nValue));
        }

        [ExcludeFromCodeCoverage]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    public class CardBackProvider : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Short circuiting is essential here.
            if (value != null
                && !(value is CardHolder) // for the main peaks where we have no binding path
                && (bool)value == false)
                return null; // hidden -> false? No image overlay
            int back = Properties.Settings.Default.Back;
            if (back < 0 || back > 7)
                back = 0;
            return string.Format(CultureInfo.InvariantCulture, "/Resources/backs/back_{0}.png", back + 1);
        }

        [ExcludeFromCodeCoverage]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
