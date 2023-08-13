using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
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
            trvResult.ItemsSource = MatchesResult;
            FlowDocument fd = new FlowDocument();
            Paragraph pr = new Paragraph();
            pr.Inlines.Add("frlopp@advantage-intentions.site\n");
            pr.Inlines.Add("marketing@sw.solarwinds.com\n");
            pr.Inlines.Add("a.smith@solarwinds.com\r\n");
            pr.Inlines.Add("noreply@t.tiki.vn\r\n");
            pr.Inlines.Add("123.234.edc.asdf@google-mail.fake-address.com-fuking\r\n");
            pr.Inlines.Add("12@gmail.com\r\n");
            pr.Inlines.Add("123..456..abc@gmail.com\r\n");
            pr.Inlines.Add("nabc.def.@gmail.com\r\n");
            pr.Inlines.Add("thiisalongnamewithmorethan30characters@gmail.com\r\n");
            pr.Inlines.Add("02053787774\r\n02053.123.456\r\n+84946157234\r\n09861452563\r\n0975-631-366\r\n" +
                "192.168.1.1\r\n8.8.8.8 8.8.4.4\r\n2001:4860:4860::8888 2001:4860:4860::8844\r\n" +
                "2001:4860:4860:0:0:0:0:8888 2001:4860:4860:0:0:0:0:8844\r\n2001:4860:4860:0000:0000:0000:0000:8888 2001:4860:4860:0000:0000:0000:0000:8844\r\n" +
                "1.1.1.1\r\n\r\n203.162.4.191 203.162.4.190\r\n\r\n203.113.131.1 203.113.131.2\r\n\r\n" +
                "208.67.222.222 208.67.220.220\r\n\r\nEmail string rule:\r\n" +
                "- Choose a username 6-30 characters long. Your username can be any combination of letters, numbers, or symbols.\r\n" +
                "- Usernames can contain letters (a-z), numbers (0-9), and periods (.).\r\n" +
                "- Usernames cannot contain an ampersand (&), equals sign (=), underscore (_), apostrophe ('), dash (-), plus sign (+), " +
                "comma (,), brackets (<,>), or more than one period (.) in a row.\r\n" +
                "- Usernames can begin or end with non-alphanumeric characters except periods (.). Other than this rule, periods (dots) don’t matter in Gmail addresses.");
            fd.Blocks.Add(pr);
            txbContent.Document = fd;
        }
        public Regex? ObjRegex { get; set; }
        public ObservableCollection<NodeInfor> MatchesResult { get; set; } = new();

        private void RichTextBoxMark(RichTextBox txbContent, int index, int length, Color foreGround, Color background)
        {
            var start = txbContent.Document.ContentStart;
            TextRange content = new TextRange(txbContent.Document.ContentStart, txbContent.Document.ContentEnd);
            content.Select(GetPos(content, index), GetPos(content, index + length));
            content.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(foreGround));
            content.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Medium);
            content.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(background));
        }

        private TextPointer GetPos(TextRange content, int index)
        {
            int i = 0;
            var pos = content.Start;
            while (i < index)
            {
                if (pos.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.None ||
                    pos.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    i++;
                }
                if (pos.GetPositionAtOffset(1, LogicalDirection.Forward) == null) //at end of document
                {
                    return pos;
                }
                pos = pos.GetPositionAtOffset(1, LogicalDirection.Forward);
            }

            return pos;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();

            }
            e.Handled = false;
        }

        private void trvResult_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            NodeInfor? ni = e.NewValue as NodeInfor;
            if (ni != null)
            {
                var content = new TextRange(txbContent.Document.ContentStart, txbContent.Document.ContentEnd);
                content.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)));
                //content.ClearAllProperties();
                RichTextBoxMark(txbContent, ni.Index, ni.Length, Colors.OrangeRed, Color.FromArgb(180,0,0,0));
            }
        }

        private void btnSearch_Click(object sender, MouseButtonEventArgs e)
        {
            if (txbPattern.Text.Length == 0) return;
            try
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pattern Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                txbPattern.Dispatcher.BeginInvoke((Action)(() => {
                    txbPattern.Focus();
                    txbPattern.SelectAll();
                }
                )) ;
                return;
            }


            MatchesResult.Clear();
            TextRange content = new TextRange(txbContent.Document.ContentStart, txbContent.Document.ContentEnd);
            //content.Select(content.Start, content.End);
            //content.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Black));
            //content.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Regular);
            content.ClearAllProperties();

            foreach (Match item in ObjRegex.Matches(content.Text))
            {

                NodeInfor ni = new NodeInfor()
                {
                    Info1 = "[M]" + item.Name,
                    Info2 = "Value: " + item.Value,
                    Info3 = "Index: " + item.Index,
                    Info4 = "Length: " + item.Length,
                    Index = item.Index,
                    Length = item.Length,
                };

                foreach (Group grp in item.Groups)
                {
                    NodeInfor sni = new NodeInfor()
                    {
                        Info1 = "[G] " + grp.Name,
                        Info2 = "Value: " + grp.Value,
                        Info3 = "Index: " + grp.Index,
                        Info4 = "Length: " + grp.Length,
                        Index = grp.Index,
                        Length = grp.Length
                    };

                    foreach (Capture cp in grp.Captures)
                    {
                        NodeInfor ssni = new NodeInfor()
                        {
                            Info1 = "[C]",
                            Info2 = "Value: " + cp.Value,
                            Info3 = "Index: " + cp.Index,
                            Info4 = "Length: " + cp.Length,
                            Index = cp.Index,
                            Length = cp.Length
                        };
                        sni.SubItems.Add(ssni);
                    }

                    ni.SubItems.Add(sni);
                }

                foreach (Capture cp in item.Captures)
                {
                    NodeInfor sni = new NodeInfor()
                    {
                        Info1 = "[C]",
                        Info2 = "Value: " + cp.Value,
                        Info3 = "Index: " + cp.Index,
                        Info4 = "Length: " + cp.Length,
                        Index = cp.Index,
                        Length = cp.Length
                    };
                    ni.SubItems.Add(sni);
                }

                MatchesResult.Add(ni);
                RichTextBoxMark(txbContent, ni.Index, ni.Length, Colors.OrangeRed, Color.FromArgb(0, 0, 0, 0));
            }
        }
    }
}
