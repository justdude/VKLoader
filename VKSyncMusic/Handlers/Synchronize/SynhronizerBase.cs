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
    public class SynhronizerBase
    {
        public bool IsHandleDataAfterReadingFromDisk = true;
        public int CountLoadedFiles { get; protected set; }
        public int FilesCount { get; protected set; }
        public int RemainCount { get; protected set; }
        public int CountThreads { get; protected set; }
        public string Path { get; protected set; }

        public delegate void DownloadProgressChangedEvent(Object sender, ProgressArgs e);

        public delegate void HandleDataEvent(IDownnloadedData data);
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

        protected HandleDataEvent mvOnUploadAction;
        public event HandleDataEvent OnUploadAction
        {
            add { mvOnUploadAction += value; }
            remove { mvOnUploadAction -= value; }
        }

        protected HandleDataEvent mvOnReadDataInfoEvent;
        public event HandleDataEvent OnReadDataInfoEvent
        {
            add { mvOnReadDataInfoEvent += (value); }
            remove { mvOnReadDataInfoEvent -= (value); }
        }

    }

}
