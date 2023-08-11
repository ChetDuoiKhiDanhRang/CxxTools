using System;
using System.Collections.Generic;
using System.Linq;
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

namespace RegexChecking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public Regex ObjRegex { get; set; }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ObjRegex = new Regex(txbPattern.Text,
                RegexOptions.None |
                ((bool)ckbCompiled.IsChecked ? RegexOptions.Compiled : RegexOptions.None) |
                ((bool)ckbCultureInvariant.IsChecked ? RegexOptions.CultureInvariant : RegexOptions.None) |
                ((bool)ckbECMAScript.IsChecked ? RegexOptions.ECMAScript : RegexOptions.None) |
                ((bool)ckbExplicitCapture.IsChecked ? RegexOptions.ExplicitCapture : RegexOptions.None) |
                ((bool)ckbIgnoreCase.IsChecked ? RegexOptions.IgnoreCase : RegexOptions.None) |
                ((bool)ckbIgnorePatternWhitespace.IsChecked ? RegexOptions.IgnorePatternWhitespace : RegexOptions.None) |
                ((bool)ckbMultiline.IsChecked ? RegexOptions.Multiline : RegexOptions.None) |
                ((bool)ckbRightToLeft.IsChecked ? RegexOptions.RightToLeft : RegexOptions.None) |
                ((bool)ckbSingleline.IsChecked ? RegexOptions.Singleline : RegexOptions.None)
            );

            foreach (Match item in ObjRegex.Matches(txbContent.Text))
            {
                item.
            }
            

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            e.Handled = false;
        }
    }
}
