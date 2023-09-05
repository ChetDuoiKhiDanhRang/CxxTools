using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RenameTool
{
    public class ItemInfo//: INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        private string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FullName"));
            }
        }

        private string nameWithoutExtension;
        public string NameWithoutExtension
        {
            get => nameWithoutExtension;
            set
            {
                nameWithoutExtension = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NameWithoutExtension"));
            }
        }

        private bool isFile;
        public bool IsFile
        {
            get => isFile;
            set
            {
                isFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFile"));
            }
        }

        private bool existed;
        public bool Existed { get => existed; set => existed = value; }

        private bool renamed = false;
        public bool Renamed { get => renamed; set => renamed = value; }

        private FileInfo FileInfo { get; set; }

        public ItemInfo? Parent { get; set; }
        
        public string Extension { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }

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
