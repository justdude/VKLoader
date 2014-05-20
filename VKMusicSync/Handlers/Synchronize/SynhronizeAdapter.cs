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
        public int CountLoadedFiles { get; private set; }
        public int FilesCount { get; private set; }
        public int RemainCount { get; private set; }
        public int CountThreads { get; private set; }
        public string Path { get; private set; }


        public delegate void DownloadProgressChangedEvent(Object sender, ProgressArgs e);

        public DownloadProgressChangedEvent OnProgress
        {
            get;
            set;
        }

        public DownloadProgressChangedEvent OnDone
        {
            get;
            set;
        }

        private ProgressArgs args
        {
            get;
            set;
        }


        public SynhronizeAdapter(String path)
        {
            this.Path = path;
            if (this.CountThreads <= 0) 
                this.CountThreads = Environment.ProcessorCount * 2;
        }

        public void SyncFolderWithList<T>(List<T> items) where T : IDownnloadedData
        {
            if (!System.IO.File.Exists(Path))
                System.IO.Directory.CreateDirectory(Path);

            Download<T>(items, Path);
        }


        private void Download<T>(List<T> existData, string directory) where T : IDownnloadedData
        {
            IOSync<T> localSync = new IOSync<T>(directory);
            localSync.ComputeFileList(existData, AudioComparer);
            List<T> valuesToDownload = localSync.GetExist();

            FilesCount = valuesToDownload.Count;
            DownloadEachFile<T>(valuesToDownload);
        }



        private void DownloadEachFile<T>(List<T> valuesToDownload) where T : IDownnloadedData
        {
            RemainCount = 0;
            for (int i = 0; i < valuesToDownload.Count; i++)
            {
                while (RemainCount>= CountThreads)
                { 
                }
                 DownloadFile(valuesToDownload[i]);
                 RemainCount++;
                 
            }
            while (RemainCount >= CountThreads)
            { }
            if (OnDone != null)
                OnDone(this, new ProgressArgs(100, 100, 0, null));
        }

        private void DownloadFile(object data) 
        {
            IDownnloadedData value = (IDownnloadedData)data;
            value.SyncState = true;
            var downloader = new Downloader(this.Path);
            downloader.OnDownloadProgressChanged += (r, e)=>
            {
                if (value.SyncState!=true)
                    value.SyncState = true;
            };
            WebClient cl = new WebClient();
            
            downloader.OnDownloadComplete += (r, e) =>
            {
                value.SyncState = false;
                RemainCount--;
                this.CountLoadedFiles++;
                if (this.OnProgress != null)
                    OnProgress(this, new ProgressArgs(1, CountLoadedFiles/(double)FilesCount, 0, null));
            };
            downloader.DownloadAsync(value.GetUrl(),
                                value.GenerateFileName() + value.GenerateFileExtention()
                               );
            
        }

        public void Stop()
        {

        }


        private bool AudioComparer<T>(System.IO.FileInfo[] files, T value) where T : IDownnloadedData
        {
            string valueFileName = value.GenerateFileName();
            return Array.Exists<System.IO.FileInfo>(files, p => (IOHandler.ParseFileName(p) == valueFileName));
        }
    }
}
