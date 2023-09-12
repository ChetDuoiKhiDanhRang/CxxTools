using BaseTools;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class MainWindow : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;

            PropertyChanged += OnPropertyChanged;
            
            LoadSettings();

            lscItems.ItemsSource = null;
            lscItems.ItemsSource = Items;

        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RegexPattern) ||
                e.PropertyName == nameof(IncludeExtension) ||
                e.PropertyName == nameof(TitleCase) ||
                e.PropertyName == nameof(CaseSensitive) ||
                e.PropertyName == nameof(ReplaceWith) ||
                e.PropertyName == nameof(ToTiengVietKhongDau))
            {
                GenerateNewName(Items);
            }

            if (e.PropertyName == nameof(IncludeFilesAndSubFolders))
            {
                Items = GenerateItemsSource(DroppedItems);
                GenerateNewName(Items);
            }

            var b1 = Validation.GetHasError(txbPattern);
            var b2 = Validation.GetHasError(txbReplaceWith);

            btnApply.IsEnabled = (!b1) && (!b2);
            btnApply.Foreground = btnApply.IsEnabled ? (new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 90, 158))) : (new SolidColorBrush(System.Windows.Media.Color.FromArgb(55, 88, 88, 88)));
            btnApply.InvalidateVisual();

            lscItems.ItemsSource = null;
            lscItems.ItemsSource = Items;
        }

        private List<KeyValuePair<string, ItemInfo>> GenerateItemsSource(List<string> files)
        {
            var Items = new Dictionary<string, ItemInfo>();
            int count = 0;
            foreach (string file in files)
            {
                var ii = ItemInfo.CreateItemInfo(file);
                ii.RootLevel = ii.Level;
                ii.IndexString = count++.ToString();

                //add the dragged in files and folders
                if (!Items.Keys.Contains(ii.FullName)) Items.Add(ii.FullName, ii);

                // if included child items
                if (IncludeFilesAndSubFolders && !ii.IsFile)
                {
                    DirectoryInfo di = new DirectoryInfo(file);

                    //add sub folders
                    foreach (DirectoryInfo directoryItem in di.GetDirectories("*", SearchOption.AllDirectories))
                    {
                        if (!Items.Keys.Contains(directoryItem.FullName))
                        {
                            ItemInfo subii = new ItemInfo(directoryItem);
                            subii.RootLevel = ii.Level;

                            //find and set parent of folder
                            subii.Parent = Items[System.IO.Path.GetDirectoryName(directoryItem.FullName)];

                            Items.Add(directoryItem.FullName, subii);
                        }
                    }

                    //add files
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

        private void GenerateNewName(List<KeyValuePair<string, ItemInfo>> items)
        {
            if (RegexPattern == "" ||
                this[nameof(ReplaceWith)].Length > 0)
                return;
            foreach (var item in items)
            {
                string targetName = IncludeExtension ? item.Value.Name : item.Value.NameWithoutExtension;

                if (UseRegex && (RegexPattern != "^") && (RegexPattern != "$"))
                {
                    Regex regex = new Regex(RegexPattern, CaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);

                    foreach (Match match in regex.Matches(targetName))
                    {
                        targetName = targetName.Replace(match.Value, ReplaceWith);
                    }
                }
                else if (RegexPattern == "^")
                {
                    targetName = ReplaceWith + targetName;
                }
                else if (RegexPattern == "$")
                {
                    targetName = targetName + ReplaceWith;
                }
                else
                {
                    targetName = targetName.Replace(RegexPattern, ReplaceWith, CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                }
                item.Value.NewName = IncludeExtension ? targetName : (targetName + item.Value.Extension);
                if (TitleCase) { item.Value.NewName = StringHandler.Proper(item.Value.NewName); }
                if (ToTiengVietKhongDau) { item.Value.NewName = StringHandler.TiengVietKhongDau(item.Value.NewName); }
            }
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
                try
                {
                    if (UseRegex && (value != "^") && (value != "$"))
                    {
                        Regex regex = new Regex(value);
                    }
                    regexPattern = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RegexPattern"));
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Wrong regex pattern!\n" + ex.Message);
                }
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

        //store dropped items
        public List<string> DroppedItems
        {
            get => droppedItems;
            set
            {
                droppedItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DroppedItems)));
            }
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                string result = "";
                
                if (columnName == nameof(ReplaceWith))
                {
                    string invalidChar = string.Join("", System.IO.Path.GetInvalidFileNameChars());
                    var invalidchars = "";
                    foreach (var c in invalidChar)
                    {
                        if (ReplaceWith.IndexOf(c) >= 0)
                        {
                            invalidchars += c;
                        }
                    }

                    return result = invalidchars.Length > 0 ? ("Invalid char: '" + invalidchars + "'") : "";
                }
                return result;
            }
        }

        List<KeyValuePair<string, ItemInfo>> items = new List<KeyValuePair<string, ItemInfo>>();

        bool useRegex = true;

        private string regexPattern ="";

        private string replaceWith ="";

        bool caseSensitive = true;

        bool titleCase = true;

        bool includeExtension = true;

        bool includeFilesAndSubFolders = true;

        bool toTiengVietKhongDau = true;

        List<string> droppedItems = new List<string>();

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
            GenerateNewName(Items);


            lscItems.ItemsSource = null;
            lscItems.ItemsSource = Items;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveSettings();
        }

        private void ckbCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in Items)
            {
                item.Value.WillBeRename = true;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WillBeRename"));
        }

        private void ckbCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in Items)
            {
                item.Value.WillBeRename = false;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WillBeRename"));
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ENABLE!");
        }
    }
}
