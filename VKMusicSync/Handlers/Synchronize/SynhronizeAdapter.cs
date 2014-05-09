using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using VKMusicSync.Model;

namespace VKMusicSync.Handlers.Synchronize
{
    public class SynhronizeAdapter
    {
        public void SyncFolderWithList<T>(List<T> sounds, string directory) where T : IData
        {
            Downloader downloader = new Downloader(directory);

            if (!System.IO.File.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            try
            {
                SyncFiles<T>(sounds, directory, downloader);
            }
            catch (System.Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message); //SPIKE!!!
            }
        }

        public DownloadDataCompletedEventHandler OnLoaded;
        public DownloadProgressChangedEventHandler OnProgress;

        private void SyncFiles<T>(List<T> values, string directory, Downloader downloader) where T : IData
        {
            Synchronizer<T> sync = new Synchronizer<T>(directory);
            sync.ComputeFileList();

            sync.Synchronize(values, CompareAudioAndFile);
            List<T> valuesToDownload = sync.GetDownloadList();

            downloader.SetOnLoaded(OnLoaded);
            downloader.SetOnChanged(OnProgress);
            DownloadEachFile<T>(downloader, valuesToDownload);
        }

        private int fileNumber;
        private int filesCount;
        public int GetDownloadedFileNumber()
        {
            return fileNumber;
        }

        public int GetDownloadedFilesCount()
        {
            return filesCount;
        }

        private void DownloadEachFile<T>(Downloader downloader, List<T> valuesToDownload) where T : IData
        {
            filesCount = valuesToDownload.Count;
            for (int i = 0; i < valuesToDownload.Count; i++)
            {
                fileNumber = i;
                downloader.Download(valuesToDownload[i].GetUrl(),
                                    valuesToDownload[i].GenerateFileName()
                                  + valuesToDownload[i].GenerateFileExtention());
            }
        }

        private bool CompareAudioAndFile<T>(System.IO.FileInfo[] files, T value) where T : IData
        {
            string valueFileName = value.GenerateFileName();
            return Array.Exists<System.IO.FileInfo>(files, p => (IOHandler.ParseFileName(p) == valueFileName));
        }
    }
}
