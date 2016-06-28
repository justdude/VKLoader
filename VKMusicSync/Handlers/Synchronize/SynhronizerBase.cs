using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VKMusicSync.Comparers;
using VKMusicSync.Delegates;
using VKMusicSync.Model;

namespace VKMusicSync.Handlers.Synchronize
{
	public abstract class SynhronizerBase
	{
		#region Properties

		public bool IsHandleDataAfterReadingFromDisk = true;
		public int CountLoadedFiles { get; protected set; }
		public int FilesCount { get; protected set; }
		public int CountThreads { get; protected set; }
		public string Path { get; protected set; }
		public ParallelOptions MultiThreadingOptions { get; set; }

		#endregion  Properties

		#region Events

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

		#endregion Events

	}

}
