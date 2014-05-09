using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VKMusicSync.Handlers.Synchronize
{

    class IOHandler
    {

        public static string ParseFileName(System.IO.FileInfo file)
        {
            return file.Name.Remove(file.Name.IndexOf(file.Extension));
        }

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
