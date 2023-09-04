using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RenameTool
{
    internal class ItemInfo
    {
        public string Name { get; set; }
        public bool IsFile { get; set; }

        public bool Exist { get; set; }

        private FileInfo FileInfo { get; set; }

        public DirectoryInfo? Parent { get; set; }
        public string Extension { get; set; }

        public ItemInfo(FileInfo file)
        {
            this.FileInfo = file;
            this.Name = file.Name;
            this.Extension = file.Extension;
            this.IsFile = true;
            this.Parent = file.Directory;
        }

    }
}
