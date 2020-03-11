using DayZ_Server_Manager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZ_Server_Manager
{
    static class DataManager
    {
        public static bool IsValidInstall
        {
            get
            {
                {
                    List<string> dirs = new List<string> { Settings.Default.DIR_Profile, Settings.Default.DIR_Server, Settings.Default.DIR_Workshop };
                    foreach (string dir in dirs)
                    {
                        if (!Directory.Exists(dir))
                        {
                            return false;
                        }
                    }

                    if (Settings.Default.FirstRun) return false;

                    return true;

                }
            }
        }
    }
}
