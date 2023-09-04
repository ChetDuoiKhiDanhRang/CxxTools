using Microsoft.Win32;
using System;
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
            Items = new Dictionary<string, FileInfo>();
            dpOptions.DataContext = this;
        }

        Dictionary<string, FileInfo> _items;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Dictionary<string, FileInfo> Items
        {
            get => _items;
            set
            {
                _items = value;
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

        public bool EntirePath
        {
            get => entirePath;
            set
            {
                entirePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntirePath)));
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

        bool useRegex;

        bool entirePath;

        bool titleCase;

        bool includeExtension;

        bool includeFilesAndSubFolders;

        bool toTiengVietKhongDau;



        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            IEnumerable<string> files = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                var fi = new FileInfo(file);
                if (!Items.Keys.Contains(fi.FullName)) Items.Add(fi.FullName, new FileInfo(file));
            }

            lscItems.ItemsSource = Items;
        }
    }
}
