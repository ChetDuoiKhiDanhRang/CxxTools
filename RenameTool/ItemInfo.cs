﻿using System;
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


        private bool willBeRename = true;
        public bool WillBeRename { 
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

        public ItemInfo(FileInfo file)
        {
            FullName = file.FullName;
            Name = file.Name;
            IsFile = true;
        }

        public ItemInfo(DirectoryInfo directoryInfo)
        {
            FullName = directoryInfo.FullName;
            Name = directoryInfo.Name;
            IsFile = false;
        }

        public static ItemInfo CreateItemInfo(string path)
        {
            var fa = System.IO.File.GetAttributes(path);
            if ((fa & FileAttributes.Directory) != FileAttributes.Directory)
            {
                FileInfo fi = new FileInfo(path);
                return new ItemInfo(fi);
            }
            else
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                return new ItemInfo(directory);
            }
        }


    }
}
