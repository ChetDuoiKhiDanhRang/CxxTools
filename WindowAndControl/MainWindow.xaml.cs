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
        App thisApp;
 
        public MainWindow()
        {
            InitializeComponent();
            thisApp = (App)App.Current;
            thisApp.MouseUpLocationChanged += ThisApp_MouseUpLocationChanged;
            App.LogFile = AppContext.BaseDirectory + @"log.txt";
        }

        private void ThisApp_MouseUpLocationChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
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
                App.UnhookWindowsHookEx();
            }
            else
            {
                App.StartKeyboardHook();
                App.StartMouseUpDownHook();
            }
            TimerStarted = !TimerStarted;
        }
    }
}
