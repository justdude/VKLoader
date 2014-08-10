using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VKMusicSync.Handlers.Synchronize
{
    class IOSync<T>
    {
        private List<T> skippedFiles;
        private DirectoryInfo dir;

        private FileInfo[] mvFolderFiles;
        public FileInfo[] FolderFiles
        {
            get
            {
                if (mvFolderFiles == null)
                {
                    UpdateFolderFiles();
                }
                return mvFolderFiles;
            }
            private set
            {
                mvFolderFiles = value;
            }
        }

        public List<T> ExistFiles
        {
            get;
            private set;
        }



        public IOSync(string dir)
        {
            this.ExistFiles = new List<T>();
            this.dir = new DirectoryInfo(dir);
            this.skippedFiles = new List<T>();
        }

        private void UpdateFolderFiles()
        {
            mvFolderFiles = dir.GetFiles();
        }

        public void CompareFolderFiles(List<T> containValues, Func<FileInfo[], T, bool> Check)
        {
            for (int i = 0; i < containValues.Count; i++)
                if (!Check(FolderFiles, containValues[i]) && !ExistFiles.Contains(containValues[i]))
                    ExistFiles.Add(containValues[i]);
        }

    }
}
