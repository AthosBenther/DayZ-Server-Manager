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
using static DayZ_Server_Manager.ModManager;
using static DayZ_Server_Manager.helpers;

namespace DayZ_Server_Manager.Controls
{
    /// <summary>
    /// Interaction logic for ModControl.xaml
    /// </summary>
    public partial class ModControl : UserControl
    {
        public ModControl(Mod mod)
        {
            InitializeComponent();

            this.Loaded += (a, b) =>
            {
                try
                {
                    Title.Text = mod.Name;
                    Updt.Text = mod.Timestamp.ToString();
                    ModInfo.ToolTip = mod.Meta.ToString2();
                }
                catch (Exception ex)
                {
                    Title.Text = "Error";
                    ModInfo.ToolTip = ex.Message;
                }
            };
        }
    }
}
