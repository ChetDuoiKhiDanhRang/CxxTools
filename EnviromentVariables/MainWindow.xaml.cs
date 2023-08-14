using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Environment;

namespace EnviromentVariables
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var variables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
            foreach (var variable in variables)
            {
                DictionaryEntry entry = (DictionaryEntry)variable;
                itemsSource.Add(new DataItem() { Name = (string)entry.Key, Content = (string)entry.Value, EnvType = "Enviroment Variable"});
            }

            var specicalFolders = Enum.GetValues(typeof(SpecialFolder)).Cast<SpecialFolder>().ToList();
            foreach (var folder in specicalFolders)
            {
                string x = Environment.GetFolderPath(folder);
                itemsSource.Add(new DataItem() { Name = folder.ToString(), Content=x, EnvType = "Special Folder"});
            }

            dataView.ItemsSource = itemsSource;

            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(dataView.ItemsSource);
            PropertyGroupDescription propertyGroupDescription = new PropertyGroupDescription(nameof(DataItem.EnvType));
            collectionView.GroupDescriptions.Add(propertyGroupDescription);
            collectionView.Filter = DataItemFilter;
        }

        private bool DataItemFilter(object obj)
        {
            if (txbFilter.Text.Length == 0) return true;
            var di = (DataItem)obj;
            return (di.Name.IndexOf(txbFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0) || 
                (di.Content.IndexOf(txbFilter.Text, StringComparison.OrdinalIgnoreCase) >=0);

        }

        public ObservableCollection<DataItem> itemsSource { get; set; } = new();

        private void txbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbFilter.Text.IndexOf("Nguyễn Ngọc Cường", StringComparison.OrdinalIgnoreCase) >=0)
            {
                Random rd = new Random();
                MessageBox.Show("Lucky number: " + rd.Next(0, 99), "LUCKY NUMBER!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataView == null || dataView.ItemsSource == null)
            {
                return;
            }
            CollectionViewSource.GetDefaultView(dataView.ItemsSource).Refresh();
        }
    }
}
