using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RenameTool
{
    class ItemInfoToTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ItemInfo i = (ItemInfo)value;
            string tooltip = "Index string: " + i.IndexString;
            tooltip += "\nName: " + i.Name;
            tooltip += "\nLocation: " + i.Location;
            tooltip += "\nNew name: " + i.NewName;
            return tooltip;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
