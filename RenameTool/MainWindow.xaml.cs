using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace RenameTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Items = new Dictionary<string, ItemInfo>();
            lscItems.ItemsSource = Items;

            LoadSettings();

            this.DataContext = this;
            //dpOptions.DataContext = this;
        }

        private void LoadSettings()
        {
            UseRegex = Properties.Settings.Default.UseRegex;
            RemoveJunkSpace = Properties.Settings.Default.RemoveJunkSpace;
            TitleCase = Properties.Settings.Default.TitleCase;
            IncludeExtension = Properties.Settings.Default.IncludeExtension;
            IncludeFilesAndSubFolders = Properties.Settings.Default.IncludeFilesAndSubFolders;
            ToTiengVietKhongDau = Properties.Settings.Default.ToTiengVietKhongDau;

            RegexPattern = Properties.Settings.Default.RegexPattern;
            ReplaceWith = Properties.Settings.Default.ReplaceWith;
        }

        private void SaveSettings()
        {
            var x = Properties.Settings.Default;
            x.UseRegex = UseRegex;
            x.RemoveJunkSpace = RemoveJunkSpace;
            x.TitleCase = TitleCase;
            x.IncludeExtension = IncludeExtension;
            x.IncludeFilesAndSubFolders = IncludeFilesAndSubFolders;
            x.ToTiengVietKhongDau = ToTiengVietKhongDau;
            x.RegexPattern = RegexPattern;
            x.ReplaceWith = ReplaceWith;
            x.Save();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Dictionary<string, ItemInfo> Items
        {
            get => items;
            set
            {
                items = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
            }
        }

        public bool UseRegex 
        { 
            get => useRegex; 
            set
            {
                useRegex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseRegex)));
            }
        }

        public string RegexPattern
        {
            get { return regexPattern; }
            set 
            { 
                regexPattern = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RegexPattern"));
            }
        }

        public string ReplaceWith
        {
            get { return replaceWith; }
            set { 
                replaceWith = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReplaceWith"));
            }
        }

        public bool RemoveJunkSpace
        {
            get => removeJunkSpace;
            set
            {
                removeJunkSpace = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RemoveJunkSpace)));
            }
        }

        public bool TitleCase
        {
            get => titleCase;
            set
            {
                titleCase = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleCase)));
            }
        }

        public bool IncludeExtension 
        { 
            get => includeExtension;
            set
            {
                includeExtension = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncludeExtension)));
            }
        }
        
        public bool IncludeFilesAndSubFolders 
        { 
            get => includeFilesAndSubFolders;
            set
            {
                includeFilesAndSubFolders = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IncludeFilesAndSubFolders)));
            }
        }
        
        public bool ToTiengVietKhongDau 
        { 
            get => toTiengVietKhongDau;
            set
            {
                toTiengVietKhongDau = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToTiengVietKhongDau)));
            }
        }

        Dictionary<string, ItemInfo> items;

        bool useRegex = true;

        private string regexPattern;

        private string replaceWith;

        bool removeJunkSpace = true;

        bool titleCase = true;

        bool includeExtension = true;

        bool includeFilesAndSubFolders = true;

        bool toTiengVietKhongDau = true;

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            IEnumerable<string> files = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                var fi = ItemInfo.CreateItemInfo(file);
                if (!Items.Keys.Contains(fi.FullName)) Items.Add(fi.FullName, fi);
            }

            lscItems.ItemsSource = null;
            lscItems.ItemsSource = Items;
            
            
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveSettings();
        }
    }
}
