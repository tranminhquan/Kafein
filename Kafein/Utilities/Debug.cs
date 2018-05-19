using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Utilities
{
    public class Debug
    {
        public static void Log(string content)
        {
            File.AppendAllText("log.txt", DateTime.Now + ": " + content + Environment.NewLine);
        }

        public static void LogOutput(string content)
        {
            System.Diagnostics.Debug.WriteLine(content);
        }
    }
}
