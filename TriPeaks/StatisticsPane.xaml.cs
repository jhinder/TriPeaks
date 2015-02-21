using System;
using System.Collections.Generic;
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
    /// Interaktionslogik für StatisticsPane.xaml
    /// </summary>
    public partial class StatisticsPane : UserControl
    {

        private StatisticsViewModel viewModel;

        public StatisticsPane()
        {
            InitializeComponent();
            viewModel = this.DataContext as StatisticsViewModel;
        }
    }

    internal class StatisticsViewModel
    {
        public int Winnings { get; set; }
        public int MostWon { get; set; }
        public int MostLost { get; set; }
        public int CurrentStreak { get; set; }
        public int SessionWinnings { get; set; }
        public int SessionAverage { get; set; }
        public int SessionGames { get; set; }
        public int PlayerGames { get; set; }
        public int PlayerAverage { get; set; }
        public int LongestStreak { get; set; }
    }

    /// <summary>
    /// Converts a streak count into the corresponding monetary value.
    /// The return value is a string of the format "n = $M(n)".
    /// </summary>
    /// <remarks>The formula is M(n) = n(n+1)/2.</remarks>
    internal class StreakToMoneyConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int n = Int32.Parse(value.ToString());
            int m = (int)((n * (n + 1)) / 2);
            return String.Format("{0} = ${1}", n, m);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

    internal class IntToDollarConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int money = Int32.Parse(value.ToString());
            return String.Format("{1}${0}", money, (money < 0 ? "-" : String.Empty));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
