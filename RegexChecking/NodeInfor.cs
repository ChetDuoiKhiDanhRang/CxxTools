using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexChecking
{
    public class NodeInfor
    {
        public int Index { get; set; }
        public int Length { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string Info3 { get; set; }
        public string Info4 { get; set; }
        public ObservableCollection<NodeInfor> SubItems { get; set; } = new();
    }
}
