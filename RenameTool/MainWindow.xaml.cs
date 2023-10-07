using BaseTools;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinAPIWrapper;
using WinAPIWrapper.ObjectInfo;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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


        bool loadDone = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            LoadSettings();

            string[] tmp_args = Environment.GetCommandLineArgs();

            if (tmp_args.Length >= 2)
            {
                for (int i = 1; i < tmp_args.Length; i++)
                {
                    if (tmp_args[i].EndsWith(System.IO.Path.DirectorySeparatorChar))
                    {
                        DroppedItems.Add(tmp_args[i].Remove(tmp_args[i].Length - 1));
                    }
                    else
                    {
                        DroppedItems.Add(tmp_args[i]);
                    }
                }
                //DroppedItems.Add("D:\\Setup\\Windows");
            }

            lscItems.ItemsSource = Items;
            lblVer.Text = "App version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "; Framework: " + AppContext.TargetFrameworkName;

            OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(DroppedItems)));
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IncludeFilesAndSubFolders) ||
                e.PropertyName == nameof(DroppedItems))
            {
                Items = GenerateItemsSource(DroppedItems);
                GenerateNewName(Items);
                lscItems.Dispatcher.Invoke(() => { lscItems.ItemsSource = null; lscItems.ItemsSource = Items; });
            }

            if (e.PropertyName == nameof(UseRegex) ||
                e.PropertyName == nameof(CaseSensitive) ||
                e.PropertyName == nameof(TitleCase) ||
                e.PropertyName == nameof(IncludeExtension) ||
                e.PropertyName == nameof(ToTiengVietKhongDau) ||
                e.PropertyName == nameof(ReplaceWith) ||
                e.PropertyName == nameof(RegexPattern)
                )
            {
                GenerateNewName(Items);
                lscItems.Dispatcher.Invoke(() => { lscItems.ItemsSource = null; lscItems.ItemsSource = Items; });
            }

            if (e.PropertyName == nameof(Items))
            {
                FilesCount = Items.Where(x => x.Value.IsFile).Count().ToString();
                FoldersCount = Items.Where(x => !x.Value.IsFile).Count().ToString();
            }

            btnApply.Dispatcher.Invoke(new Action(() =>
            {
                btnApply.IsEnabled = !((this[nameof(RegexPattern)].Length > 0 || this[nameof(ReplaceWith)].Length > 0));// || ToTiengVietKhongDau;
                btnApply.Foreground = btnApply.IsEnabled ? (new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 90, 158))) : (new SolidColorBrush(System.Windows.Media.Color.FromArgb(55, 88, 88, 88)));
            }));
        }

        private List<KeyValuePair<string, ItemInfo>> GenerateItemsSource(List<string> droppedItems)
        {
            var Items = new Dictionary<string, ItemInfo>();
            int count = 0;
            foreach (string droppedItem in droppedItems)
            {
                var item = ItemInfo.CreateItemInfo(droppedItem);
                item.RootLevel = item.Level;
                item.IndexString = count++.ToString();

                //add the dragged in files and folders
                if (!Items.Keys.Contains(item.FullName)) Items.Add(item.FullName, item);

                // if included child items
                if (IncludeFilesAndSubFolders && !item.IsFile)
                {
                    DirectoryInfo di = new DirectoryInfo(droppedItem);

                    //add sub folders
                    foreach (DirectoryInfo directoryItem in di.GetDirectories("*", SearchOption.AllDirectories))
                    {
                        if (!Items.Keys.Contains(directoryItem.FullName))
                        {
                            ItemInfo subii = new ItemInfo(directoryItem);
                            subii.RootLevel = item.Level;

                            //set parent of folder
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
                            subii.RootLevel = item.Level;
                            subii.Parent = Items[System.IO.Path.GetDirectoryName(fileInfo.FullName)];
                            Items.Add(fileInfo.FullName, subii);
                        }

                    }
                }
            }
            var result = new List<KeyValuePair<string, ItemInfo>>(Items.OrderBy(k => k.Value.IndexString));

            return result;
        }

        private void GenerateNewName(List<KeyValuePair<string, ItemInfo>> items)
        {
            if (
                //RegexPattern == "" ||
                this[nameof(ReplaceWith)].Length > 0 ||
                this[nameof(RegexPattern)].Length > 0 ||
                items.Count == 0
               ) return;
            foreach (var item in items)
            {
                Task.Run(() =>
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
                });

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


        bool useRegex = true;
        public bool UseRegex
        {
            get => useRegex;
            set
            {
                useRegex = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(UseRegex)));
            }
        }

        private string regexPattern = "";
        public string RegexPattern
        {
            get { return regexPattern; }
            set
            {
                regexPattern = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs("RegexPattern"));
            }
        }

        private string replaceWith = "";
        public string ReplaceWith
        {
            get { return replaceWith; }
            set
            {
                replaceWith = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs("ReplaceWith"));
            }
        }

        bool caseSensitive = true;
        public bool CaseSensitive
        {
            get => caseSensitive;
            set
            {
                caseSensitive = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(CaseSensitive)));
            }
        }

        bool titleCase = true;
        public bool TitleCase
        {
            get => titleCase;
            set
            {
                titleCase = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(TitleCase)));
            }
        }

        bool includeExtension = true;
        public bool IncludeExtension
        {
            get => includeExtension;
            set
            {
                includeExtension = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(IncludeExtension)));
            }
        }

        bool includeFilesAndSubFolders = true;
        public bool IncludeFilesAndSubFolders
        {
            get => includeFilesAndSubFolders;
            set
            {
                includeFilesAndSubFolders = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(IncludeFilesAndSubFolders)));
            }
        }

        bool toTiengVietKhongDau = true;
        public bool ToTiengVietKhongDau
        {
            get => toTiengVietKhongDau;
            set
            {
                toTiengVietKhongDau = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(ToTiengVietKhongDau)));
            }
        }

        //store dropped items
        List<string> droppedItems = new List<string>();
        public List<string> DroppedItems
        {
            get => droppedItems;
            set
            {
                droppedItems = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(DroppedItems)));
            }
        }

        List<KeyValuePair<string, ItemInfo>> items = new List<KeyValuePair<string, ItemInfo>>();
        public List<KeyValuePair<string, ItemInfo>> Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Items)));
            }
        }

        private string filesCount;

        public string FilesCount
        {
            get { return filesCount; }
            set
            {
                filesCount = value;
                //OnPropertyChanged(this, new PropertyChangedEventArgs(nameof (FilesCount)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilesCount)));
            }
        }

        private string foldersCount;

        public string FoldersCount
        {
            get { return foldersCount; }
            set
            {
                foldersCount = value;
                //OnPropertyChanged(this, new PropertyChangedEventArgs(nameof (FoldersCount)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoldersCount)));
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
                else if (columnName == nameof(RegexPattern))
                {
                    if (RegexPattern == "") result = "Empty pattern!";

                    if (UseRegex && RegexPattern != "$" && RegexPattern != "^")
                    {

                        try
                        {
                            Regex rg = new Regex(RegexPattern);
                        }
                        catch (Exception ex)
                        {
                            result = "Wrong regex pattern\n" + ex.Message;
                        }
                    }
                }

                return result;
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

            DroppedItems = files;
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
            ApplyNewName();
            //Items.Clear();
            //Items = GenerateItemsSource(DroppedItems);
            //GenerateNewName(Items);


            lscItems.ItemsSource = null;
            lscItems.ItemsSource = Items;

            this.Activate();

        }

        private void ApplyNewName()
        {
            var RenameList = Items.Where(x => x.Value.WillBeRename).ToList();
            if (!RenameList.Any()) return;

            var fileList = RenameList.Where(x => x.Value.IsFile).ToList();
            var folderList = RenameList.Where(x => !x.Value.IsFile).OrderByDescending(x => x.Value.Level).ToList();
            List<string> newDroppedItems = new List<string>();

            foreach (var file in fileList)
            {
                var fileInfo = new FileInfo(file.Value.FullName);
                if (fileInfo.Exists)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(file.Value.NewName))
                        {
                            string newName = file.Value.Location + System.IO.Path.DirectorySeparatorChar + file.Value.NewName;
                            if (!File.Exists(newName))
                            { fileInfo.MoveTo(newName); }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fail rename file: " + file.Key + "\n" + ex.Message + "\nCLOSE ALL PROGRAMS AND TRY AGAIN!",
                                "ERROR",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        this.Close();
                        Application.Current?.Shutdown();
                    }
                    foreach (var item in DroppedItems)
                    {
                        if (item == file.Key) { newDroppedItems.Add(fileInfo.FullName); break; }
                    }
                }
            }


            //List openning window explorer
            var processExplorer = Process.GetProcessesByName("explorer");
            List<WindowInfo> explorerWindows = new List<WindowInfo>();
            if (processExplorer != null)
            {
                foreach (var process in processExplorer)
                {
                    explorerWindows.AddRange(WinAPI.GetWindowsByProcessID((uint)process.Id).Where(w => !string.IsNullOrEmpty(w.Title) && w.IsVisible).ToArray());
                }
            }


            foreach (var folder in folderList)
            {
                var dirInfo = new DirectoryInfo(folder.Value.FullName);
                if (dirInfo.Exists)
                {
                    //Handle the Windows Explorer openning the rename-folder or child folder of it
                    if (explorerWindows.Count > 0)
                    {
                        foreach (var window in explorerWindows)
                        {
                            string s = GetOpenningAddress(window);
                            while (GetOpenningAddress(window).Contains(folder.Key))
                            {
                                Up2Parent(window);
                                window.Update();
                            }
                        }

                    }

                    //Rename folder
                    if (!string.IsNullOrEmpty(folder.Value.NewName) &&
                        (string.Compare(folder.Value.Name, (folder.Value.NewName), true) != 0))
                    {
                        try
                        {
                            string newPath = (folder.Value.Location + System.IO.Path.DirectorySeparatorChar + folder.Value.NewName).Replace("\\\\", "\\");
                            if (!Directory.Exists(newPath)) dirInfo.MoveTo(newPath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Fail rename folder: " + folder.Key + "\n" + ex.Message + "\nCLOSE ALL OTHER PROGRAM AND TRY AGAIN!",
                                "ERROR",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            //this.Close();
                            //Application.Current.Shutdown();
                        }

                        foreach (var item in DroppedItems)
                        {
                            if (item.Replace(@"\\", @"\") == folder.Key)
                            {
                                newDroppedItems.Add((folder.Value.Location + System.IO.Path.DirectorySeparatorChar + folder.Value.NewName).Replace(@"\\", @"\"));
                                break;
                            }
                        }
                    }
                }
            }

            if (newDroppedItems.Count > 0)
            {
                DroppedItems.Clear();
                DroppedItems = newDroppedItems;
            }
        }

        private string GetOpenningAddress(WindowInfo windowInfo)
        {
            var controlAddress = windowInfo.GetControlsByText("Address: ").FirstOrDefault();
            if (controlAddress == null)
            {
                return "";
            }
            else
            {
                return controlAddress.CaptionText.Remove(0, "Address: ".Length);
            }

        }

        private void Up2Parent(WindowInfo windowInfo)
        {
            var BtnUp = windowInfo.GetControlsByText("Up band").FirstOrDefault();
            if (BtnUp != null)
            {
                WinAPI.SendLeftClickToControl(BtnUp.Handle);
            }
        }

        private void lscItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lscItems.SelectedItem != null)
            {
                KeyValuePair<string, ItemInfo> i = (KeyValuePair<string, ItemInfo>)lscItems.SelectedItem;
                string path = i.Key;
                Process.Start("explorer", path);
            }
        }

    }
}
