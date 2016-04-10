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

        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            IconHelper.RemoveIcon(this);
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

        private readonly string assemblyVersion = "Version 1.0";
        private readonly string assemblyCopyright = "&#169; dfragment.net 2015-2016";

        public AboutDialogViewModel()
        {
            var copyrightAttrs = thisAssembly.GetCustomAttributes<AssemblyCopyrightAttribute>();
            var versionAttrs = thisAssembly.GetCustomAttributes<AssemblyFileVersionAttribute>();
            // AssemblyVersionAttribute is not added to the assembly metadata.

            if (copyrightAttrs.Any())
                assemblyCopyright = copyrightAttrs.First().Copyright;
            if (versionAttrs.Any())
                assemblyVersion = $"Version {versionAttrs.First().Version}";
        }

        public string Version
        {
            get { return assemblyVersion; }
        }

        public string Copyright
        {
            get { return assemblyCopyright; }
        }

    }

}
