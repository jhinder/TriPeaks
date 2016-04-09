using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace TriPeaks
{
    /// <summary>
    /// Interaktionslogik für BackSelectDialog.xaml
    /// </summary>
    public partial class BackSelectDialog : Window
    {

        private BackSelectViewModel viewModel;

        public BackSelectDialog()
        {
            InitializeComponent();
            viewModel = DataContext as BackSelectViewModel;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }

        private void SaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.Save();
            Close();
        }

        private void SetBackExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.SelectedBack = int.Parse(e.Parameter.ToString());
        }

        private void CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }

    /// <summary>
    /// The view model for the back selection dialog.
    /// </summary>
    public class BackSelectViewModel : INotifyPropertyChanged
    {

        private int _selectedBack;
        /// <summary>
        /// The index of the selected card back.
        /// </summary>
        public int SelectedBack
        {
            get { return _selectedBack; }
            set
            {
                _selectedBack = value;
                RaisePropertyChanged();
            }
        }

        private Properties.Settings settings;

        public BackSelectViewModel()
        {
            settings = Properties.Settings.Default;

            try {
                SelectedBack = settings.Back;
            } catch (InvalidCastException) {
                SelectedBack = 0;
            }

            if (SelectedBack < 0 || SelectedBack >= 8)
                SelectedBack = 0;
        }

        public void Save()
        {
            settings.Back = SelectedBack;
            settings.Save();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Converts the selected card back index to a border thickness (to show or hide the border)
    /// </summary>
    public class IndexToBorderConverter : IMultiValueConverter
    {

        private static readonly SolidColorBrush blackColour = new SolidColorBrush(Colors.Black);
        private static readonly SolidColorBrush whiteColour = new SolidColorBrush(Colors.White);

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            /* First object: selected index (int)
             * Second object: button index (int) */
            if (values == null || values.Length < 2)
                return blackColour;
            return (values[0].ToString() == values[1].ToString()) ? blackColour : whiteColour;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
