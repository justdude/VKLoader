using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VKLib.Delegates;
using VKMusicSync.Comparers;
using VKMusicSync.Delegates;

namespace VKMusicSync.Handlers.Synchronize
{
	public partial class SynhronizingProvider<T> : SynhronizerBase where T : IDownnloadedData, IEqualityComparer<T>
	{
		#region Fields

		private readonly IoSync<T> ioSync;

		#endregion Fields

		#region Properties

		public List<T> ComputedFileList { get; private set; }

		public string FileExtension { get; set; }

		#endregion Properties

		#region Ctr.

		public SynhronizingProvider(IoSync<T> ioSync, ParallelOptions multiThreadingOptions)
		{
			this.ioSync = ioSync;

			MultiThreadingOptions = multiThreadingOptions;
		}

		#endregion Ctr.

		#region Methods

		public void ComputeFileList(Func<T> creator, Func<List<T>> downloadItemsListFromWeb)
		{
			List<T> existData = ioSync.CastExistData(creator);

			//read from tags
			#region Tags from disk
			//if (IsHandleDataAfterReadingFromDisk) //&& OnReadDataInfoEvent != null)
			//{
			//	foreach(var item in existData)
			//	{
			//	   ReadDataInfoAction(item, mvOnReadDataInfoEvent);
			//	}
			//} 
			#endregion

			if (downloadItemsListFromWeb == null)
			{
				ComputedFileList = existData;
				return;
			}

			//load data from modLoader and merge
			IEqualityComparer<T> comparer = new GenericComparer<T>();
			List<T> internetData = downloadItemsListFromWeb();
			List<T> mergedData = existData.Union(internetData, comparer).ToList();
			foreach (var item in mergedData)
			{
				item.State = SyncStates.Unknown;
				bool isExistLocal = existData.Exists(p => p.Equals(item));
				bool isExistRemote = internetData.Exists(p => p.Equals(item));

				SetFileListItemStatus(item, isExistLocal, isExistRemote);

			}
			ComputedFileList = mergedData;
		}


		private static void SetFileListItemStatus(T item, bool isExistLocal, bool isExistRemote)
		{
			if (isExistLocal && isExistRemote)
				item.State = SyncStates.Synced;
			else if (isExistLocal)
			{
				item.State = SyncStates.NeedUpload;
			}
			else if (isExistRemote)
			{
				item.State = SyncStates.NeedDownloaded;
			}
		}

		public async void SyncFolderWithListAsync<T>(string path, List<T> items) where T : IDownnloadedData, IStateChanged
		{
			Path = path;

			if (!File.Exists(Path))
				Directory.CreateDirectory(Path);

			await Task.Run(() => Parallel.ForEach(items, MultiThreadingOptions, HandleActionsWithItem));

			if (OnDone != null)
				OnDone(this, new ProgressArgs(0, 100));
		}


		private void HandleActionsWithItem<T>(T item) where T : IDownnloadedData, IStateChanged
		{
			switch (item.State)
			{
				case SyncStates.NeedDownloaded:
					DownloadFile(item);
					break;
				case SyncStates.NeedUpload:
					UploadFile(item, mvOnUploadAction);
					break;
				case SyncStates.Synced:
					ActionsDelegates.Execute(item.OnLoadEnded,
						this,
						new Argument() { result = true });

					break;
				case SyncStates.SyncFailed:
					ActionsDelegates.Execute(item.OnRaiseError,
						this,
						new Argument() { result = true });

					break;
			}
		}

		#endregion Methods

		public string Path { get; set; }
	}




}
