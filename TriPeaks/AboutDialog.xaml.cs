using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace TriPeaks
{
    /// <summary>
    /// Interaktionslogik für AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog()
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

        private async void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            await Dispatcher.InvokeAsync((Action)(() =>
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }));
            
        }
    }
}
