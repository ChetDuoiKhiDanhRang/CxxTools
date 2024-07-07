using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenameTool.Models
{
    class Item
    {
        public bool IsFile { get; set; } = false;
        public string? Name { get; set; }
        public string? Extension { get; set; }
        public string? NewName { get; set; }
        public int Level { get; set; }
        public bool WilBeRenamed { get; set; }

    }
}
