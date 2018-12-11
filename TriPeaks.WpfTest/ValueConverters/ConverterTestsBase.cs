using System;
using System.Drawing;
using System.Windows.Data;

namespace TriPeaks.WpfTest.ValueConverters
{
    public abstract class ConverterTestsBase<TConverter>
        where TConverter : IValueConverter, new()
    {
        private protected static readonly Type StringType = typeof(string);
        private protected static readonly Type ColorType = typeof(Color);

        private protected readonly TConverter converter;

        protected ConverterTestsBase()
        {
            converter = new TConverter();
        }
    }
}
