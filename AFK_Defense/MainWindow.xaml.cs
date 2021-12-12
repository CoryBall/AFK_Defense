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

namespace AFK_Defense
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _running = false;
        private string _processName = "ffxiv_dx11";

        public MainWindow()
        {
            InitializeComponent();

            if (!ProcessHelper.CheckIfProcessIsRunning(_processName))
            {
                lblStatus.Content = "FFXIV is not running.";
                btnAction.IsEnabled = false;
            }
        }

        private async void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            if (_running)
            {
                this.lblStatus.Content = "Playing";
                this.lblStatus.Foreground = Brushes.Green;
                this.btnAction.Content = "BRB";
                _running = false;
            }
            else
            {
                this.lblStatus.Content = "AFK";
                this.lblStatus.Foreground = Brushes.Red;
                this.btnAction.Content = "I'm Back";
                _running = true;

                await Task.Run(() =>
                {
                    while (_running)
                    {
                        ProcessHelper.SetForeGroundMainWindowHandle(_processName);
                        KeyPresser.TargetCancel();
                        //KeyPresser.RunInCircle();
                        // Wait 1 min
                        System.Threading.Thread.Sleep(1000 * 60 * 20);
                    }
                });
            }
        }
    }
}
