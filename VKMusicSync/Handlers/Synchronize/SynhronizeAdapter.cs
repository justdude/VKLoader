using System;
using System.Collections;
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

        private ArrayList downloaders = new ArrayList();

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

        public SynhronizeAdapter(String path, int threadsCount)
        {
            this.Path = path;
            this.CountThreads = threadsCount;
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
            localSync.CompareFolderFiles(existData, AudioComparer);
            List<T> valuesToDownload = localSync.ExistFiles;

            FilesCount = valuesToDownload.Count;
            DownloadEachFile<T>(valuesToDownload);
        }


        private bool controlDownloading = false;
        private void DownloadEachFile<T>(List<T> valuesToDownload) where T : IDownnloadedData
        {
            controlDownloading = false;
            RemainCount = 0;

            for (int i = 0; i < valuesToDownload.Count; i++)
            {
                if (controlDownloading == true)
                    break;
                while (RemainCount>= CountThreads)
                { 
                }
                 DownloadFile(valuesToDownload[i]);
                 RemainCount++;
                 
            }
            while (RemainCount > 0)
            { }
            if (OnDone != null)
                OnDone(this, new ProgressArgs(100, 100, 0, null));
        }

        public void CancelDownloading()
        {
            controlDownloading = true;
            /*while (RemainCount >= CountThreads)
            { }*/
            for (int i = 0; i < downloaders.Count; i++)
            {
                var target = (Downloader)downloaders[i];
                target.CancelAsync();
                downloaders.Remove(target);
                target = null;
            }
        }

        

        private void DownloadFile(object data) 
        {
            IDownnloadedData value = (IDownnloadedData)data;
            var songItem = (VKMusicSync.ModelView.SoundModelView)data;


            value.SyncState = true;
            var downloader = new Downloader(this.Path);
            downloaders.Add(downloader);

            downloader.OnDownloadProgressChanged += (r, e)=>
            {
                if (value.SyncState!=true)
                    value.SyncState = true;
                if (songItem.Checked == false)
                    songItem.Checked = true;
                if (this.OnProgress != null)
                    OnProgress(this, new ProgressArgs(1, CountLoadedFiles / (double)FilesCount, 0, null));
            };

            downloader.OnDownloadComplete += (r, e) =>
            {
                
                RemainCount--;
                CountLoadedFiles++;
                downloaders.Remove(downloader);

                songItem.Checked = false;
                value.SyncState = false;

                if (this.OnProgress != null)
                    OnProgress(this, new ProgressArgs(1, CountLoadedFiles/(double)FilesCount, 0, null));
            };

            downloader.DownloadAsync(value.GetUrl(),
                                value.GenerateFileName() + value.GenerateFileExtention()
                               );
        }


        private bool AudioComparer<T>(System.IO.FileInfo[] files, T value) where T : IDownnloadedData
        {
            string valueFileName = value.GenerateFileName();
            return Array.Exists<System.IO.FileInfo>(files, p => (IOHandler.ParseFileName(p) == valueFileName));
        }
    }
}
