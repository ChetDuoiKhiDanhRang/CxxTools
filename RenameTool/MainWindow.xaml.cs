using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
            //Items = new Dictionary<string, ItemInfo>();

            LoadSettings();

            this.DataContext = this;


            lscItems.ItemsSource = null;
            lscItems.ItemsSource = Items;

            PropertyChanged += OnPropertyChanged;
            
            //dpOptions.DataContext = this;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(IncludeFilesAndSubFolders))
            {
                GenerateItemsSource(DroppedItems);
            }
        }

        private List<KeyValuePair<string,ItemInfo>> GenerateItemsSource(List<string> files)
        {
            var Items = new Dictionary<string,ItemInfo>();
            int count = 0;
            foreach (string file in files)
            {
                var ii = ItemInfo.CreateItemInfo(file);
                ii.RootLevel = ii.Level;
                ii.IndexString = count++.ToString();
                if (!Items.Keys.Contains(ii.FullName)) Items.Add(ii.FullName, ii);
                if (IncludeFilesAndSubFolders && !ii.IsFile)
                {
                    DirectoryInfo di = new DirectoryInfo(file);
                    foreach (DirectoryInfo subdi_item in di.GetDirectories("*", SearchOption.AllDirectories))
                    {
                        if (!Items.Keys.Contains(subdi_item.FullName))
                        {
                            ItemInfo subii = new ItemInfo(subdi_item);
                            subii.RootLevel = ii.Level;
                            subii.Parent = Items[System.IO.Path.GetDirectoryName(subdi_item.FullName)];

                            Items.Add(subdi_item.FullName, subii);
                        }
                    }

                    foreach (FileInfo fileInfo in di.GetFiles("*", SearchOption.AllDirectories))
                    {
                        if (!Items.Keys.Contains(fileInfo.FullName))
                        {
                            ItemInfo subii = new ItemInfo(fileInfo);
                            subii.RootLevel = ii.Level;
                            subii.Parent = Items[System.IO.Path.GetDirectoryName(fileInfo.FullName)];
                            Items.Add(fileInfo.FullName, subii);
                        }

                    }
                }
            }
            var result = Items.OrderBy(k => k.Value.IndexString).ToList();
            return result;
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

        public List<KeyValuePair<string, ItemInfo>> Items
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
            set
            {
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


        List<KeyValuePair<string, ItemInfo>> items = new List<KeyValuePair<string, ItemInfo>>();

        bool useRegex = true;

        private string regexPattern;

        private string replaceWith;

        bool caseSensitive = true;

        bool titleCase = true;

        bool includeExtension = true;

        bool includeFilesAndSubFolders = true;

        bool toTiengVietKhongDau = true;



        //store dropped items
        List<string> droppedItems = new List<string>();
        public List<string> DroppedItems
        {
            get => droppedItems;
            set
            {
                droppedItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DroppedItems)));
            }
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }
        private void Window_Drop(object sender, DragEventArgs e)
        {
            List<string> files = ((IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop)).ToList();
            
            if (files.Count() == 0) return;

            files.Sort();

            DroppedItems.Clear();
            DroppedItems.AddRange(files);
            Items = GenerateItemsSource(DroppedItems);





        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveSettings();
        }
    }
}
