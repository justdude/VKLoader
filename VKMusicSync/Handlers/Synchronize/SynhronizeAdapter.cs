using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using VKMusicSync.Model;

namespace VKMusicSync.Handlers.Synchronize
{
    public class SynhronizeAdapter
    {
        public int CurrentFileNumber { get; private set; }
        public int FilesCount { get; private set; }
        public string Path
        { get; private set; }

        public DownloadDataCompletedEventHandler OnLoaded { get; set; }
        public DownloadProgressChangedEventHandler OnProgress { get; set; }


        public void SyncFolderWithList<T>(List<T> items, string path) where T : IDownnloadedData
        {
            this.Path = path;
            Downloader downloader = new Downloader(Path);

            if (!System.IO.File.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            try
            {
                SyncFiles<T>(items, path, downloader);
            }
            catch (System.Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message); //SPIKE!!!
            }
        }

        private void SyncFiles<T>(List<T> existData, string directory, Downloader downloader) where T : IDownnloadedData
        {
            IOSync<T> localSync = new IOSync<T>(directory);
            localSync.ComputeFileList(existData, AudioComparer);
            List<T> valuesToDownload = localSync.GetExist();

            downloader.OnDownloadComplete += OnLoaded;
            downloader.OnDownloadProgressChanged += OnProgress;
            DownloadEachFile<T>(downloader, valuesToDownload);
        }

        private void DownloadEachFile<T>(Downloader downloader, List<T> valuesToDownload) where T : IDownnloadedData
        {
            FilesCount = valuesToDownload.Count;
            ThreadPool.SetMaxThreads(4, 3);

            for (int i = 0; i < valuesToDownload.Count; i++)
            {
                CurrentFileNumber = i;
                Process<T>(valuesToDownload[i]);   
                /*ThreadPool.QueueUserWorkItem( new WaitCallback( (object arg)=>{
                    int n = (int)arg;
                    CurrentFileNumber = n;
                    Process<T>(valuesToDownload[n]);
                }));*/

                //WaitHandle.WaitAll()
            }
        }

        private void Process<T>(T value) where T : IDownnloadedData
        {
            value.SyncState = true;
            var downloader = new Downloader(this.Path);
            downloader.Download(value.GetUrl(),
                                value.GenerateFileName() + value.GenerateFileExtention()
                              );
            value.SyncState = false;
        }

        private bool AudioComparer<T>(System.IO.FileInfo[] files, T value) where T : IDownnloadedData
        {
            string valueFileName = value.GenerateFileName();
            return Array.Exists<System.IO.FileInfo>(files, p => (IOHandler.ParseFileName(p) == valueFileName));
        }
    }
}
