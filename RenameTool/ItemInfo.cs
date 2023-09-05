using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RenameTool
{
    public class ItemInfo
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public string NameWithoutExtension { get; set; }

        public bool IsFile { get; set; }

        public bool Exist { get; set; }

        private FileInfo FileInfo { get; set; }

        public ItemInfo? Parent { get; set; }
        public string Extension { get; set; }

        private bool renamed = false;
        public bool Renamed { get => renamed; set => renamed = value; }

        public ItemInfo(FileInfo file)
        {
            this.FileInfo = file;
            this.Name = file.Name;
            this.NameWithoutExtension = Path.GetFileNameWithoutExtension(file.FullName);
            this.Extension = file.Extension;
            this.IsFile = true;
            this.FullName = file.FullName;
            Renamed = true;
        }

    }
}
