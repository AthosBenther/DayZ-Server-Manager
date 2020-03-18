using DayZ_Server_Manager.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DayZ_Server_Manager
{
    /// <summary>
    /// Interaction logic for ServerCfg.xaml
    /// </summary>
    public partial class ServerCfg : Window
    {
        public ServerCfg()
        {
            InitializeComponent();

            this.Loaded += ServerCfg_Loaded;
            this.Closing += ServerCfg_Closing;
        }

        private void ServerCfg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Config.Default.Save();
            StartParams.Default.Save();
            GenConfigFile();
        }

        private void ServerCfg_Loaded(object sender, RoutedEventArgs e)
        {
            LoadParams();
            LoadCfgs();
            SetMissions();

        }

        private void LoadParams()
        {
            TabItem Item = CfgStage.Items[0] as TabItem;

            StackPanel Stacks = Item.Content as StackPanel;

            Stacks.Children.OfType<StackPanel>().ToList().ForEach(s =>
            {
                s.Children.OfType<TextBox>().ToList().ForEach(p =>
                {
                    if (p.Name != "port")
                    {
                        p.Text = StartParams.Default[p.Name] as string;
                        p.TextChanged += P_TextChanged;
                    }
                    else
                    {
                        p.Text = StartParams.Default.port.ToString();
                    }
                });

                s.Children.OfType<CheckBox>().ToList().ForEach(p =>
                {
                    p.IsChecked = (StartParams.Default[p.Name] as bool?) == true;
                    p.Click += P_Click;
                });
                s.Children.OfType<Slider>().ToList().ForEach(p =>
                {
                    p.Value = (double)(StartParams.Default[p.Name] as int?);
                    p.ValueChanged += P_ValueChanged;
                });
            });
        }

        private void LoadCfgs()
        {
            int items = CfgStage.Items.Count;

            for (int i = 1; i < items; i++)
            {
                TabItem Item = CfgStage.Items[i] as TabItem;

                StackPanel Stacks = Item.Content as StackPanel;


                Stacks.Children.OfType<StackPanel>().ToList().ForEach(s =>
                {
                    s.Children.OfType<TextBox>().ToList().ForEach(c =>
                    {
                        c.Text = Config.Default[c.Name] as string;

                        c.TextChanged += C_TextChanged;
                    });
                    s.Children.OfType<CheckBox>().ToList().ForEach(c =>
                    {
                        c.IsChecked = Config.Default[c.Name] as bool?;
                        c.Click += C_Click;
                    });
                    s.Children.OfType<Slider>().ToList().ForEach(c =>
                    {
                        c.Value = (double)(Config.Default[c.Name] as int?);
                        c.ValueChanged += C_ValueChanged;
                    });
                });

            }
        }

        private void SetMissions()
        {
            List<string> Missions = Directory.EnumerateDirectories(Settings.Default.DIR_Server + "\\mpmissions").ToList().ConvertAll<string>(d => d.Replace(Settings.Default.DIR_Server + "\\mpmissions\\", ""));

            Missions.ForEach(m =>
            {
                ComboBoxItem c = new ComboBoxItem();
                c.Content = m;
                template.Items.Add(c);
            });

            if (Missions.Contains(Config.Default.template))
            {
                template.SelectedIndex = Missions.IndexOf(Config.Default.template);
            }
            else
            {
                template.SelectedIndex = 0;
            }
        }

        private void P_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox p = sender as TextBox;

            StartParams.Default[p.Name] = p.Text;
        }

        private void port_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void port_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                port.BorderBrush = Brushes.Black;
                port.Background = Brushes.Transparent;
                int p = int.Parse(port.Text);
                StartParams.Default.port = p;
            }
            catch (Exception ex)
            {
                port.BorderBrush = Brushes.Red;
                port.Background = Brushes.LightPink;
                port.ToolTip = ex.Message;
            }
        }

        private void P_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider p = sender as Slider;

            StartParams.Default[p.Name] = (int)p.Value;
        }

        private void P_Click(object sender, RoutedEventArgs e)
        {
            CheckBox p = sender as CheckBox;

            StartParams.Default[p.Name] = p.IsChecked == true;
        }


        private void C_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider c = sender as Slider;

            Config.Default[c.Name] = (int)c.Value;
        }

        private void C_Click(object sender, RoutedEventArgs e)
        {
            CheckBox c = sender as CheckBox;

            Config.Default[c.Name] = c.IsChecked == true;
        }

        private void C_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox c = sender as TextBox;

            Config.Default[c.Name] = c.Text;
        }

        private void template_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Config.Default.template = (template.SelectedItem as ComboBoxItem).Content.ToString();
        }

        public static void GenConfigFile()
        {
            string cfg = "";
            foreach (SettingsProperty currentProperty in Properties.Config.Default.Properties)
            {
                switch (currentProperty.Name)
                {
                    case "motd":
                        string motd = "{\"";
                        string motdraw = Config.Default[currentProperty.Name] as string;
                        motdraw.Split('\n').ToList().ForEach(l =>
                        {
                            if (!string.IsNullOrWhiteSpace(l.Trim('\n').Trim('\r')))
                            {
                                motd += l.Trim('\n').Trim('\r') + "\",\"";
                            }
                        });
                        motd = motd.TrimEnd('"');
                        motd = motd.TrimEnd(',');
                        motd += "}";
                        cfg += "motd[] = " + motd + ";\n";
                        break;
                    case "timeStampFormat":
                        cfg += (Config.Default["timeStampFormat"] == "true") ? "timeStampFormat = Full" : "timeStampFormat = Short";
                        break;
                    case "template":
                        break;
                    default:
                        cfg = cfg + currentProperty.Name + " = " + Config.Default[currentProperty.Name] + ";\n";
                        break;
                }
            }

            cfg += "\n\n\n";

            cfg += "class Missions{ class DayZ { template=\"" + Config.Default.template + "\";}}";

            try
            {
                File.WriteAllText(Settings.Default.DIR_Server + "\\DZSM.cfg", cfg);
            }
            catch (Exception ex)
            {
                var a = MessageBox.Show("Your settings were saved and the file can be generated later", "Error saving the config file");
            }


        }
    }
}
