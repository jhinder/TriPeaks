using System;
using System.Collections.Generic;

namespace TriPeaks.WpfTest.ValueConverters
{
    internal static class EnumUtils
    {
        public static IEnumerable<T> GetValues<T>()
            where T : struct, Enum
        {
            foreach (T item in Enum.GetValues(typeof(T)))
                yield return item;
        }
    }
}
