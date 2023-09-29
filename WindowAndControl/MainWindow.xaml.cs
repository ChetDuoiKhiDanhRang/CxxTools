using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BaseTools;
using BaseTools.Win32API_Objects;

namespace WindowAndControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer timer;
        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer(50);
            timer.Elapsed += Timer_Elapsed;
        }

        CursorInfo ci;

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            ci = BaseTools.WinAPI.GetCursorInfo();
            txbLocation.Dispatcher.Invoke(() => txbLocation.Text = $"{{{ci.ToString()}}}");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Title = "Mouse Down!";
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Title = "Mouse Up!";

        }

        public bool TimerStarted { get; set; }


        private void btnTrackMouse_Click(object sender, RoutedEventArgs e)
        {
            if (TimerStarted)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
            TimerStarted = !TimerStarted;
        }
    }
}
