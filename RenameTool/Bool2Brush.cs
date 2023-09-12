using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RenameTool
{
    internal class Bool2Brush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (bool)value;
            Brush brush;
            if (v)
            {
                brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 90, 158));
            }
            else
            {
                brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(55, 88, 88, 88));
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
