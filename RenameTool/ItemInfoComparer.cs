using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenameTool
{
    internal class ItemInfoComparer : IComparer<KeyValuePair<string, ItemInfo>>
    {
        public int Compare(KeyValuePair<string, ItemInfo> x, KeyValuePair<string, ItemInfo> y)
        {
            return x.Value.CompareTo(y.Value);
        }
    }
}
