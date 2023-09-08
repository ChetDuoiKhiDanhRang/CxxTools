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
    public class ItemInfo : IComparable//: INotifyPropertyChanged
    {

        private string nameWithoutExtension;

        public string NameWithoutExtension
        {
            get { return nameWithoutExtension; }
            set
            {
                nameWithoutExtension = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(NameWithoutExtension)));
            }
        }


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

        private bool isFile;
        public bool IsFile
        {
            get => isFile;
            private set
            {
                isFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFile"));
            }
        }

        private string location = "";

        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Location)));
            }
        }


        private string newName;

        public string NewName
        {
            get { return newName; }
            set
            {
                newName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(NewName)));
            }
        }


        private int rootLevel = 0;

        public int RootLevel
        {
            get { return rootLevel; }
            set
            {
                rootLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RootLevel)));
            }
        }



        private int level = 0;

        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Level)));
            }
        }


        private bool willBeRename = true;
        public bool WillBeRename
        {
            get => willBeRename;
            set
            {
                willBeRename = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(WillBeRename)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);

        }

        public ItemInfo? Parent { get; set; }

        public ItemInfo(FileInfo fileInfo)
        {
            FullName = fileInfo.FullName;
            Name = fileInfo.Name;
            IsFile = true;
            Location = Path.GetDirectoryName(fileInfo.FullName);
            Level = FullName.Where(x => x == Path.DirectorySeparatorChar).Count();
            NameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
        }

        public ItemInfo(DirectoryInfo directoryInfo)
        {
            FullName = directoryInfo.FullName;
            Name = directoryInfo.Name;
            IsFile = false;
            Location = Path.GetDirectoryName(directoryInfo.FullName);
            Level = FullName.Where(x => x == Path.DirectorySeparatorChar).Count();
            NameWithoutExtension = Name;
        }

        public static ItemInfo CreateItemInfo(string path)
        {
            if (path == null) return null;
            var fa = System.IO.File.GetAttributes(path);
            if ((fa & FileAttributes.Directory) != FileAttributes.Directory)
            {
                FileInfo fi = new FileInfo(path);
                return new ItemInfo(fi);
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(path);
                return new ItemInfo(di);
            }
        }

        public int CompareTo(object? obj)
        {
            var o = (ItemInfo)obj;
            if (o.Location == Location)
            {
                return IsFile.CompareTo(o.IsFile);
            }
            return Location.CompareTo(o.Location);
        }
    }
}
