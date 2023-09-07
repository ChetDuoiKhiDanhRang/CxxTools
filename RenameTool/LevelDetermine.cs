using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RenameTool
{
    class LevelDetermine : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ItemInfo i = (ItemInfo)value;

            if (i.Parent == null) return new Thickness(0, 0, 0, 0);

            Thickness tk = new Thickness((i.Level - i.Parent.Level)*8D, 0D,0D,0D);

            return tk;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
