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
    public class ItemInfo: INotifyPropertyChanged
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


        private string extension;
        public string Extension
        {
            get { return extension; }
            set
            {
                extension = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Extension)));
            }
        }



        private string name;
        public string Name
        {
            get
            {
                return NameWithoutExtension + Extension;
            }
            private set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        private string fullName;
        public string FullName
        {
            get
            {
                return (Location + Path.DirectorySeparatorChar + Name).Replace("\\\\", "\\");
            }
            private set
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

        private ItemInfo? parent;
        public ItemInfo? Parent
        {
            get => parent;
            set
            {
                if (value != null && parent != null)
                {

                    parent.Childs.Remove(this);
                }

                parent = value;
                parent.Childs.Add(this);
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Parent)));
            }
        }

        private List<ItemInfo> childs;
        public List<ItemInfo> Childs
        {
            get => childs;
            set
            {
                childs = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Childs)));
            }
        }

        private string indexString = "";

        public string IndexString
        {
            get
            {
                return indexString;
            }
            set
            {
                indexString = value;
            }
        }



        public ItemInfo(FileInfo fileInfo)
        {
            IsFile = true;
            NameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            Extension = Path.GetExtension(fileInfo.FullName);
            Location = Path.GetDirectoryName(fileInfo.FullName);
            Level = FullName.Where(x => x == Path.DirectorySeparatorChar).Count();
            Childs = new List<ItemInfo>();
            PropertyChanged += ItemInfo_PropertyChanged;
        }

        private void ItemInfo_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Parent) && Parent != null) IndexString = Parent.IndexString + "." + (IsFile ? "fi" : "fo") + Parent.Childs.Count();
        }

        public ItemInfo(DirectoryInfo directoryInfo)
        {
            IsFile = false;
            NameWithoutExtension = (directoryInfo.Name);
            Extension = "";
            Location = Path.GetDirectoryName(directoryInfo.FullName);
            Level = FullName.Where(x => x == Path.DirectorySeparatorChar).Count();
            Childs = new List<ItemInfo>();
            PropertyChanged += ItemInfo_PropertyChanged;
        }

        public static ItemInfo CreateItemInfo(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
            {
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
            return null;
        }

    }
}
