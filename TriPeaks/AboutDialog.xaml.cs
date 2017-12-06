using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

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

        private void CloseExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private async void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            await Dispatcher.InvokeAsync(() => {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            });   
        }
    }

    internal class AboutDialogViewModel
    {

        private static readonly Assembly thisAssembly = typeof(AboutDialogViewModel).Assembly;

        public AboutDialogViewModel()
        {
            var copyrightAttrs = thisAssembly.GetCustomAttributes<AssemblyCopyrightAttribute>();
            var versionAttrs = thisAssembly.GetCustomAttributes<AssemblyFileVersionAttribute>();
            // AssemblyVersionAttribute is not added to the assembly metadata.

            if (copyrightAttrs.Any())
                Copyright = copyrightAttrs.First().Copyright;
            if (versionAttrs.Any())
                Version = $"Version {versionAttrs.First().Version}";
        }

        public string Version { get; } = "Version 1.0";

        public string Copyright { get; } = "&#169; dfragment.net 2015-2016";
    }

}
