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
using System.Data;
using System.Diagnostics;
using System.IO;
using DayZ_Server_Manager.Properties;
using static DayZ_Server_Manager.ModManager;
using DayZ_Server_Manager.Controls;

namespace DayZ_Server_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            if (!DataManager.IsValidInstall)
            {
                Window cfg = new ConfigPaths();
                cfg.ShowDialog();
            }

            foreach (Mod mod in ModManager.ServerMods)
            {
                ServerModsStage.Children.Add(new ModControl(mod));
            }

            foreach (Mod mod in ModManager.ClientMods)
            {
                ClientModsStage.Children.Add(new ModControl(mod));
            }

        }

        private void ServerCfgBtn_Click(object sender, RoutedEventArgs e)
        {
            Window SvCfg = new ServerCfg();
            SvCfg.ShowDialog();
        }
    }
}