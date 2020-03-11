using DayZ_Server_Manager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZ_Server_Manager
{
    public static class ModManager
    {
        public static List<Mod> ServerMods => GetMods(Settings.Default.DIR_Server);
        public static List<Mod> ClientMods => GetMods(Settings.Default.DIR_Workshop);

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
            public Exception Error;
            public Mod(string path)
            {
                Path = path;
                string MetaFile = Path + "\\Meta.cpp";

                try
                {
                    List<string> Meta = File.ReadLines(MetaFile).ToList();
                    string NameLine = Meta.Find(l => l.StartsWith("name"));
                    Name = NameLine.Substring(NameLine.LastIndexOf("=") + 3).TrimEnd(new char[] { ';', '"' });
                }
                catch (Exception ex)
                {
                    Error = ex;
                }

            }
        }
    }
}
