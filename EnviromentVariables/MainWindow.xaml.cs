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
                itemsSource.Add(new DataItem() { Name = (string)entry.Key, Content = (string)entry.Value, DataType="Enviroment Variable" });
            }

            var specicalFolder = Enum.GetValues(typeof(SpecialFolder)).Cast<SpecialFolder>().ToList();
            foreach (var specical in specicalFolder)
            {
                string x = Environment.GetFolderPath(specical);
                itemsSource.Add(new DataItem() { Name = specical.ToString(), Content=x, DataType="Enviroment Special Folder" });
            }

            datagrid.ItemsSource = itemsSource;

            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(datagrid.ItemsSource);
            PropertyGroupDescription propertyGroupDescription = new PropertyGroupDescription("DataType");
            collectionView.GroupDescriptions.Add(propertyGroupDescription);


        }

        public ObservableCollection<DataItem> itemsSource { get; set; } = new();
    }
}
