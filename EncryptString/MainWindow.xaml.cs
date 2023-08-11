using System;
using System.Collections.Generic;
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
using CxxBaseTools;

namespace EncryptString
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

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Name == btnText2Hex.Name)
            {
                txb_Hex.Text = StringHandler.StringToHexcode(txb_Input.Text, Encoding.UTF8);
            }
            else if (btn.Name == btnHex2Text.Name)
            {
                try
                {
                    txb_Input.Text = StringHandler.HexcodeToString(txb_Hex.Text, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    txb_Input.Text = ex.Message;
                }
            }
        }

        private void txb_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            var txb = (TextBox)sender;
            txb.Text += e.Data.GetData("Text").ToString();
        }

        private void txb_Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            tblk_MD5.Text = Encryptor.EncryptString(textBox.Text, Encoding.UTF8, Encryptor.HashTypes.MD5);
            tblk_SHA1.Text = Encryptor.EncryptString(textBox.Text, Encoding.UTF8, Encryptor.HashTypes.SHA1);
            tblk_SHA256.Text = Encryptor.EncryptString(textBox.Text, Encoding.UTF8, Encryptor.HashTypes.SHA256);
            tblk_SHA384.Text = Encryptor.EncryptString(textBox.Text, Encoding.UTF8, Encryptor.HashTypes.SHA384);
            tblk_SHA512.Text = Encryptor.EncryptString(textBox.Text, Encoding.UTF8, Encryptor.HashTypes.SHA512);

        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            //Clipboard.Clear();
            if (bt.Name == btnCopy_MD5.Name)
            {
                Clipboard.SetText(tblk_MD5.Text, TextDataFormat.Text);
                tblk_MD5.SelectAll();
                statusItem1.Content = "MD5 text copied!";
            }
            else if (bt.Name == btnCopy_SHA1.Name)
            {
                Clipboard.SetText(tblk_SHA1.Text, TextDataFormat.Text);
                tblk_SHA1.SelectAll();
                statusItem1.Content = "SHA1 text copied!";

            }
            else if (bt.Name == btnCopy_SHA256.Name)
            {
                Clipboard.SetText(tblk_SHA256.Text, TextDataFormat.Text);
                tblk_SHA256.SelectAll();
                statusItem1.Content = "SHA256 text copied!";
            }
            else if (bt.Name == btnCopy_SHA384.Name)
            {
                Clipboard.SetText(tblk_SHA384.Text, TextDataFormat.Text);
                tblk_SHA384.SelectAll();
                statusItem1.Content = "SHA384 text copied!";
            }
            else if (bt.Name == btnCopy_SHA512.Name)
            {
                Clipboard.SetText(tblk_SHA512.Text, TextDataFormat.Text);
                tblk_SHA512.SelectAll();
                statusItem1.Content = "SHA512 text copied!";
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void txb_Input_KeyDown(object sender, KeyEventArgs e)
        {
            statusItem1.Content = "Key down: " + e.Key.ToString();
        }
    }
}
