using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VKMusicSync.Handlers.Synchronize
{
    class Synchronizer<T>
    {
        private List<T> existFiles;
        private List<T> skippedFiles;

        private FileInfo[] folderFiles;
        private DirectoryInfo dir;

        public Synchronizer(string dir)
        {
            if (Directory.Exists(dir))
            {
                this.existFiles = new List<T>();
                this.dir = new DirectoryInfo(dir);
                this.skippedFiles = new List<T>();
            }
            else
            {
                throw new Exception("Folder doesnt exist");
            }
        }

        public void ComputeFileList()
        {
            folderFiles = dir.GetFiles();
        }

        public void Synchronize(List<T> containValues, Func<FileInfo[], T, bool> Check)
        {
            for (int i = 0; i < containValues.Count; i++)
                if (!Check(folderFiles, containValues[i]) && !existFiles.Contains(containValues[i]))
                    existFiles.Add(containValues[i]);
        }


        public List<T> GetDownloadList()
        {
            return existFiles;
        }

    }
}
