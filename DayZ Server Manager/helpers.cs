using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZ_Server_Manager
{
    public static class helpers
    {
        public static string ToString2(this List<string> input)
        {
            string output = "";

            input.ForEach(l => output = output + l + "\n");

            return output;
            
        }
    }
}
