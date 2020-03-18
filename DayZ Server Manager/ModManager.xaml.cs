using DayZ_Server_Manager.Controls;
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
    /// Interaction logic for ModManager.xaml
    /// </summary>
    public partial class ModManager : Window
    {
        public static List<Mod> ServerMods => GetMods(Settings.Default.DIR_Server);
        public static List<Mod> ClientMods => GetMods(Settings.Default.DIR_Workshop);

        public ModManager()
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
        private static List<Mod> GetMods(string Folder)
        {
            List<Mod> Mods = new List<Mod>();
            var dirs = Directory.EnumerateDirectories(Folder).ToList();
            var mods = dirs.FindAll(f => f.Substring(f.LastIndexOf('\\') + 1).StartsWith("@"));

            foreach (var mod in mods)
            {
                Mods.Add(new Mod(mod));
            }
            return Mods;
        }
        public class Mod
        {
            public string Path, Name;
            public int PublishedID;
            public DateTime Timestamp;
            public List<string> Meta;
            public Exception Error;
            public Mod(string path)
            {
                Path = path;
                string MetaFile = Path + "\\Meta.cpp";

                try
                {
                    Meta = File.ReadLines(MetaFile).ToList();
                    Timestamp = File.GetLastWriteTimeUtc(MetaFile);
                    Name = GetMetaString("name");
                    PublishedID = GetMetaInt("publishedid");
                }
                catch (Exception ex)
                {
                    Error = ex;
                }

            }

            private string GetMetaString(string MetaProperty)
            {
                string NameLine = Meta.Find(l => l.StartsWith(MetaProperty));
                return NameLine.Substring(NameLine.LastIndexOf("=") + 3).TrimEnd(new char[] { ';', '"' });
            }
            private int GetMetaInt(string MetaProperty)
            {
                string NameLine = Meta.Find(l => l.StartsWith(MetaProperty));
                string inttxt = NameLine.Substring(NameLine.LastIndexOf("=") + 2).TrimEnd(new char[] { ';', '"' });
                return Int32.Parse(inttxt);
            }
        }
    }
}

