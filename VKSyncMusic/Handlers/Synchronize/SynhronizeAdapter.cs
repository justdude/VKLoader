using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using VKSyncMusic.Comparers;
using VKSyncMusic.Delegates;
using VKSyncMusic.Model;

namespace VKSyncMusic.Handlers.Synchronize
{
    public class SynhronizeProcessor<T> : SynhronizerBase where T : IDownnloadedData, IEqualityComparer<T>
    {
        private ArrayList downloaders = new ArrayList();
        private IOSync<T> handler = null;

        private bool controlDownloading = false;

        private List<T> mvComputedFileList;
        public List<T> ComputedFileList
        {
            get
            {
                return mvComputedFileList;
            }
        }

				public static SynhronizeProcessor<T> Create()
				{
					return new SynhronizeProcessor<T>(Properties.Settings.Default.DownloadFolderPath,
								".mp3",
								Properties.Settings.Default.ThreadCountToUse);
				}


        public SynhronizeProcessor(String path, string fileExtension, int threadsCount) 
        {
            base.Path = path;
            base.CountThreads = threadsCount;
            this.FileExtension = fileExtension;
            this.handler = new IOSync<T>(this.Path, FileExtension);
        }

        public void ComputeModList(Func<T> Creator, Func<List<T>> DownloadFromWeb)
        {
            mvComputedFileList = ComputeFileList(Creator, DownloadFromWeb);
        }

        

        private List<T> ComputeFileList(Func<T> Creator, Func<List<T>> DownloadInfoFromWeb)
        {
            handler.UpdateData();
            List<T> existData = handler.CastExistData(Creator);

            //read from tags
            if (IsHandleDataAfterReadingFromDisk) //&& OnReadDataInfoEvent != null)
            {
                foreach(var item in existData)
                {
                   ReadDataInfoAction(item, mvOnReadDataInfoEvent);
                }
            }

            //load data from modLoader and merge
            if (DownloadInfoFromWeb != null)
            {
                IEqualityComparer<T> comparer = new GenericComparer<T>();
                List<T> internetData = DownloadInfoFromWeb();
                List<T> mergedData = existData.Union(internetData, comparer).ToList();
                foreach( var item in mergedData)
                {
                    item.State = SyncStates.Default;
                    bool isExistLocal = existData.Exists(p => p.Equals(item));
                    bool isExistRemote = internetData.Exists( p=> p.Equals(item));

                    SetFileListItemStatus(item, isExistLocal, isExistRemote);

                }
                return mergedData;
            }
            return existData;
        }


        private static void SetFileListItemStatus(T item, bool isExistLocal, bool isExistRemote)
        {
            if (isExistLocal && isExistRemote)
                item.State = SyncStates.IsSynced;
            else if (isExistLocal)
            {
                item.State = SyncStates.IsNeedUpload;
            }
            else if (isExistRemote)
            {
                item.State = SyncStates.IsNeedDownload;
            }
        }


        //public void SyncFolderWithList<T>(List<T> items) where T : IDownnloadedData
        //{
        //    if (!System.IO.File.Exists(Path))
        //        System.IO.Directory.CreateDirectory(Path);

        //    Download<T>(items, Path);
        //}

        public void SyncFolderWithList<T>(List<T> items) where T : IDownnloadedData
        {
            if (!System.IO.File.Exists(Path))
                System.IO.Directory.CreateDirectory(Path);

            ProcessEachFile<T>(items);
        }


        //private void Download<T>(List<T> existData, string directory) where T : IDownnloadedData
        //{
        //    IOSync<T> localSync = new IOSync<T>(directory, this.FileExtension);
        //    localSync.CompareFolderFiles(existData, AudioComparer);
        //    List<T> valuesToDownload = localSync.ExistData;

        //    base.FilesCount = valuesToDownload.Count;
        //    DownloadEachFile<T>(valuesToDownload);
        //}

        private void ProcessEachFile<T>(List<T> valuesToDownload) where T : IDownnloadedData
        {
            controlDownloading = false;
            base.RemainCount = 0;

            for (int i = 0; i < valuesToDownload.Count; i++)
            {
                if (controlDownloading == true)
                    break;
                while (RemainCount >= CountThreads)
                {
                    Thread.Sleep(50);
                }
                var cachedData = valuesToDownload[i];

                HandleActionsWithItem<T>(cachedData);

                base.RemainCount++;

            }
            while (RemainCount > 0)
            {
                Thread.Sleep(50);
            }
            if (OnDone != null)
                OnDone(this, new ProgressArgs(100, 100, 0, null));
        }

        private void HandleActionsWithItem<T>(T item) where T : IDownnloadedData
        {
            if (item.State == SyncStates.IsNeedDownload)
                DownloadFile(item);


            else if (item.State == SyncStates.IsNeedUpload)
            {
                UploadFile(item, mvOnUploadAction);
            }

            else if (item.State == SyncStates.IsSynced)
            {
                ActionsDelegates.Execute(item.InstanceWithEvents.OnLoadEnded,
                         this,
                         new Delegates.Argument() { result = true });

                OnDownloadCompleteActions(item, null);
            }

            else if (item.State == SyncStates.IsCanntUpdate)
            {
                ActionsDelegates.Execute(item.InstanceWithEvents.OnRaiseError,
                         this,
                         new Delegates.Argument() { result = true });

                OnDownloadCompleteActions(item, null);
            }
        }

        //private void DownloadEachFile<T>(List<T> valuesToDownload) where T : IDownnloadedData
        //{
        //    controlDownloading = false;
        //    base.RemainCount = 0;

        //    for (int i = 0; i < valuesToDownload.Count; i++)
        //    {
        //        if (controlDownloading == true)
        //            break;
        //        while (RemainCount>= CountThreads)
        //        { 
        //        }
        //         DownloadFile(valuesToDownload[i]);
        //         base.RemainCount++;
                 
        //    }
        //    while (RemainCount > 0)
        //    { }
        //    if (OnDone != null)
        //        OnDone(this, new ProgressArgs(100, 100, 0, null));
        //}

        public void CancelDownloading()
        {
            controlDownloading = true;
            /*while (RemainCount >= CountThreads)
            { }*/
            for (int i = 0; i < downloaders.Count; i++)
            {
                var target = (DataLoader)downloaders[i];
                target.CancelAsync();
                downloaders.Remove(target);
                target = null;
            }
        }



        private void DownloadFile(IDownnloadedData value) 
        {
            var downloader = new DataLoader(this.Path);
            downloaders.Add(downloader);

            downloader.OnDownloadProgressChanged += (r, e)=>
            {

                ActionsDelegates.Execute(value.InstanceWithEvents.OnProgresChanged, 
                                         this, 
                                         new Delegates.Argument() {  result = false});

                if (this.OnProgress != null)
                    OnProgress(this, 
                               new ProgressArgs(1, 
                               CountLoadedFiles / (double)FilesCount, 0, null));
            };

            downloader.OnDownloadComplete += (r, e) =>
            {
                ActionsDelegates.Execute(value.InstanceWithEvents.OnLoadEnded, 
                                         this, 
                                         new Delegates.Argument() { result = true });

                OnDownloadCompleteActions(value, downloader);

                if (this.OnProgress != null)
                    OnProgress(this, 
                               new ProgressArgs(1, CountLoadedFiles/(double)FilesCount, 
                               0,
                               null));
            };

            ActionsDelegates.Execute(value.InstanceWithEvents.OnLoadStarted,
                                     this, 
                                     new Delegates.Argument() { result = false });

            downloader.DownloadAsync(value.Path, value.FileName + " " + value.FileExtention);
        }

        private void ReadDataInfoAction(IDownnloadedData item, HandleDataEvent OnReadDataInfoAction)
        {
            if (OnReadDataInfoAction != null)
                OnReadDataInfoAction(item);
        }

        private void UploadFile(IDownnloadedData value, HandleDataEvent OnUploadAction)
        {
            ActionsDelegates.Execute(value.InstanceWithEvents.OnLoadStarted,
                         this,
                         new Delegates.Argument() { result = false });


            if (OnUploadAction != null)
                OnUploadAction(value);


            ActionsDelegates.Execute(value.InstanceWithEvents.OnLoadEnded,
                                        this,
                                        new Delegates.Argument() { result = true });

            OnDownloadCompleteActions(value, null);

           /* var downloader = new DataLoader(this.Path);
            downloaders.Add(downloader);

            downloader.OnUploadComplete += (r, e) =>
            {
                ActionsDelegates.Execute(value.InstanceWithEvents.OnLoadEnded,
                                         this,
                                         new Delegates.Argument() { result = true });

                OnDownloadCompleteActions(value, downloader);

                if (this.OnProgress != null)
                    OnProgress(this,
                               new ProgressArgs(1, CountLoadedFiles / (double)FilesCount,
                               0,
                               null));
            };

            ActionsDelegates.Execute(value.InstanceWithEvents.OnLoadStarted,
                                     this,
                                     new Delegates.Argument() { result = false });
            */
            //downloader.UploadAsync(value.Path, value.FileName + " " + value.FileExtention);

        }

        private void OnDownloadCompleteActions(IDownnloadedData value, DataLoader downloader)
        {
            base.RemainCount--;
            base.CountLoadedFiles++;
            downloaders.Remove(downloader);
        }


        private bool AudioComparer<T>(System.IO.FileInfo[] files, T value) where T : IDownnloadedData
        {
            string valueFileName = value.FileName +" "+ value.FileExtention;
            return Array.Exists<System.IO.FileInfo>(files, p => (IOHandler.ParseFileName(p) == valueFileName));
        }

        public string FileExtension { get; set; }
    }



    
}
