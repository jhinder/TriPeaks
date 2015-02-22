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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            viewModel = this.DataContext as BackSelectViewModel;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }

        private void SaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.Save();
            this.Close();
        }

        private void SetBackExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.SelectedBack = Int32.Parse(e.Parameter.ToString());
        }

        private void CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
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
                RaisePropertyChanged("SelectedBack");
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
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Converts the selected card back index to a border thickness (to show or hide the border)
    /// </summary>
    public class IndexToBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value.ToString() == parameter.ToString()) ? 2 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
