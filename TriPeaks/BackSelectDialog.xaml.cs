using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        private readonly BackSelectViewModel viewModel;

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
            bool didParse = int.TryParse(e.Parameter.ToString(), out var newBack);
            viewModel.SelectedBack = didParse ? newBack : 0;
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
        private readonly Properties.Settings settings;

        /// <summary>
        /// The index of the selected card back.
        /// </summary>
        public int SelectedBack
        {
            get { return _selectedBack; }
            set
            {
                if (value < 0 || value >= 8)
                    value = 0;
                _selectedBack = value;
                RaisePropertyChanged();
            }
        }

        public BackSelectViewModel()
        {
            settings = Properties.Settings.Default;

            try
            {
                SelectedBack = settings.Back;
            }
            catch (InvalidCastException)
            {
                SelectedBack = 0;
            }
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
}
