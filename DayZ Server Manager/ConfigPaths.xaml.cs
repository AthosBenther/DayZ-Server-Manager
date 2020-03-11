using DayZ_Server_Manager.Properties;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace DayZ_Server_Manager
{
    /// <summary>
    /// Interaction logic for ConfigPaths.xaml
    /// </summary>
    public partial class ConfigPaths : Window
    {
        public ConfigPaths()
        {
            InitializeComponent();

            ServerInput.Text = Settings.Default.DIR_Server;
            ServerInput.BorderBrush = Directory.Exists(Settings.Default.DIR_Server) ? Brushes.Black : Brushes.Red;

            WorkshopInput.Text = Settings.Default.DIR_Workshop;
            WorkshopInput.BorderBrush = Directory.Exists(Settings.Default.DIR_Workshop) ? Brushes.Black : Brushes.Red;

            ProfileInput.Text = Settings.Default.DIR_Profile;
            ProfileInput.BorderBrush = Directory.Exists(Settings.Default.DIR_Profile) ? Brushes.Black : Brushes.Red;


            ValidateOK();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox input = sender as TextBox;
            input.BorderBrush = Directory.Exists(input.Text) ? Brushes.Black : Brushes.Red;

            ValidateOK();
        }

        private void ValidateOK()
        {
            if(fields.Children.OfType<TextBox>().Any<TextBox>(t => t.BorderBrush == Brushes.Red))
            {
                Ok.IsEnabled = false;
            }
            else
            {
                Ok.IsEnabled = true;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.DIR_Profile = ProfileInput.Text;
            Settings.Default.DIR_Workshop = WorkshopInput.Text;
            Settings.Default.DIR_Server = ServerInput.Text;
            Settings.Default.FirstRun = false;

            Settings.Default.Save();
            this.Close();
        }
    }
}
