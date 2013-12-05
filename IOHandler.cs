using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace VK
{
    class IOHandler
    {

        public static void Write(string path, string value)
        {
            System.IO.File.WriteAllText(path, value);
        }

        public static void ClearFolder(string dir) 
        {
            foreach (string file in Directory.GetFiles(dir))
                File.Delete(file);
        }

        public static void OpenPath(string dir)
        {
            System.Diagnostics.Process.Start(dir);
        }
    }
}
