using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public partial class MainWindow : Window, INotifyPropertyChanged, INotifyCollectionChanged
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
            CaseSensitive = Properties.Settings.Default.CaseSensitive;
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
            x.CaseSensitive = CaseSensitive;
            x.TitleCase = TitleCase;
            x.IncludeExtension = IncludeExtension;
            x.IncludeFilesAndSubFolders = IncludeFilesAndSubFolders;
            x.ToTiengVietKhongDau = ToTiengVietKhongDau;
            x.RegexPattern = RegexPattern;
            x.ReplaceWith = ReplaceWith;
            x.Save();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public Dictionary<string, ItemInfo> Items
        {
            get => items;
            set
            {
                items = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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

        public bool CaseSensitive
        {
            get => caseSensitive;
            set
            {
                caseSensitive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CaseSensitive)));
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

        bool caseSensitive = true;

        bool titleCase = true;

        bool includeExtension = true;

        bool includeFilesAndSubFolders = true;

        bool toTiengVietKhongDau = true;

        ObservableCollection<ItemInfo> itemInfos;

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            IEnumerable<string> files = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);

            if (files.Count() == 0) return;

            Items.Clear();

            foreach (string file in files)
            {
                var ii = ItemInfo.CreateItemInfo(file);
                ii.RootLevel = ii.Level;
                if (!Items.Keys.Contains(ii.FullName)) Items.Add(ii.FullName, ii);
                if (IncludeFilesAndSubFolders && !ii.IsFile)
                {
                    DirectoryInfo di = new DirectoryInfo(file);
                    foreach (DirectoryInfo item in di.GetDirectories("*", SearchOption.AllDirectories))
                    {
                        if (!Items.Keys.Contains(item.FullName))
                        {
                            Items.Add(item.FullName, new ItemInfo(item) { RootLevel = ii.Level});
                        }

                        foreach (FileInfo fileInfo in di.GetFiles("*", SearchOption.AllDirectories))
                        {
                            if (!Items.Keys.Contains(fileInfo.FullName))
                            {
                                Items.Add(fileInfo.FullName, new ItemInfo(fileInfo) { RootLevel = ii.Level });
                            }    

                        }
                    }

                    foreach (FileInfo item in di.GetFiles())
                    {
                        if (!Items.Keys.Contains(item.FullName))
                        {
                            Items.Add(item.FullName, new ItemInfo(item) { RootLevel = ii.Level});
                        }    
                    }
                    
                }    
            }

            var source = Items.ToList();
            source.Sort(new ItemInfoComparer());

            lscItems.ItemsSource = null;
            lscItems.ItemsSource = source;
            
            
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveSettings();
        }
    }
}
