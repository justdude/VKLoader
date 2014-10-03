using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VKMusicSync.Model;

namespace VKMusicSync.Handlers
{
    public static class DataHelper<T> where T : IDownnloadedData
    {

        public static void ConvertFrom<T>(FileInfo fileInfo, T element) where T : IDownnloadedData
        {
            element.Path = fileInfo.DirectoryName;
            //.FileExtention = fileInfo.Extension;
            element.FileName = fileInfo.Name.Replace( fileInfo.Extension, "");
            element.MD5 = fileInfo.GetHashCode().ToString();
            element.IsLoadedToDisk = false;
            element.State = Synchronize.SyncStates.Default;
            element.LoadedSize = (double)fileInfo.Length;
        }

        public static List<T> CastInfoToList(FileInfo[] FilesInfo, Func<T> Creator)
        {
            if (FilesInfo == null && Creator != null)
                return null;

            List<T> tempColl = new List<T>();
            foreach (var folderInfo in FilesInfo)
            {
                T el = Creator();
                if (el != null)
                {
                    ConvertFrom<T>(folderInfo, el);
                    tempColl.Add(el);
                }

            }
            return tempColl;
        }
    }
}
