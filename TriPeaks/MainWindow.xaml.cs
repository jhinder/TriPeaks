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

        public MainWindow()
        {
            InitializeComponent();
            viewModel = this.DataContext as TriPeaksViewModel;
        }

        #region Commands

        private void DealCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private async void ChangePlayerExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await AsyncDialog<PlayerNameDialog>();
        }

        private void ResetGameExecuted(object sender, ExecutedRoutedEventArgs e)
        {

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
