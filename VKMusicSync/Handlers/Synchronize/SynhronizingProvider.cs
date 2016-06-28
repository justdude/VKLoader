using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VKLib.Delegates;
using VKMusicSync.Comparers;
using VKMusicSync.Delegates;
using VKMusicSync.Model;

namespace VKMusicSync.Handlers.Synchronize
{
	public class SynhronizingProvider<T> : SynhronizerBase where T : IDownnloadedData, IEqualityComparer<T>
	{
		#region Fields

		private IoSync<T> handler = null;

		private List<T> mvComputedFileList;

		#endregion Fields

		#region Properties

		public List<T> ComputedFileList
		{
			get
			{
				return mvComputedFileList;
			}
		}

		public string FileExtension { get; set; }

		#endregion Properties

		#region Ctr.

		public SynhronizingProvider(IoSync<T> ioSync, ParallelOptions options)
		{
			this.handler = ioSync;

			base.MultiThreadingOptions = options;
		}

		#endregion Ctr.

		#region Methods

		public void ComputeFileList(Func<T> creator, Func<List<T>> downloadItemsListFromWeb)
		{
			List<T> existData = handler.CastExistData(creator);

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

			//load data from modLoader and merge
			if (downloadItemsListFromWeb != null)
			{
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
				mvComputedFileList = mergedData;
				return;
			}
			mvComputedFileList = existData;
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

		public async void SyncFolderWithListAsync<T>(List<T> items) where T : IDownnloadedData
		{
			if (!System.IO.File.Exists(Path))
				System.IO.Directory.CreateDirectory(Path);

			await Task.Run(() => Parallel.ForEach(items, MultiThreadingOptions, HandleActionsWithItem));

			if (OnDone != null)
				OnDone(this, new ProgressArgs(100, 100, 0, null));
		}


		private void HandleActionsWithItem<T>(T item) where T : IDownnloadedData
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
					ActionsDelegates.Execute(item.InstanceWithEvents.OnLoadEnded,
						this,
						new Argument() { result = true });

					break;
				case SyncStates.SyncFailed:
					ActionsDelegates.Execute(item.InstanceWithEvents.OnRaiseError,
						this,
						new Argument() { result = true });

					break;
			}
		}

		private void DownloadFile(IDownnloadedData value)
		{
			var downloader = new DataLoader(this.Path);

			downloader.OnDownloadProgressChanged += (r, e) =>
			{

				ActionsDelegates.Execute(value.InstanceWithEvents.OnProgresChanged,
										 this,
										 new Argument() { result = false });

				if (this.OnProgress != null)
					OnProgress(this,
							   new ProgressArgs(1,
							   CountLoadedFiles / (double)FilesCount, 0, null));
			};

			downloader.OnDownloadComplete += (r, e) =>
			{
				ActionsDelegates.Execute(value.InstanceWithEvents.OnLoadEnded,
										 this,
										 new Argument() { result = true });

				if (this.OnProgress != null)
					OnProgress(this,
							   new ProgressArgs(1, CountLoadedFiles / (double)FilesCount,
							   0,
							   null));
			};

			ActionsDelegates.Execute(value.InstanceWithEvents.OnLoadStarted,
									 this,
									 new Argument() { result = false });

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
						 new Argument() { result = false });


			if (OnUploadAction != null)
				OnUploadAction(value);


			ActionsDelegates.Execute(value.InstanceWithEvents.OnLoadEnded,
										this,
										new Argument() { result = true });

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


		private bool AudioComparer<T>(System.IO.FileInfo[] files, T value) where T : IDownnloadedData
		{
			string valueFileName = value.FileName + " " + value.FileExtention;
			return Array.Exists<System.IO.FileInfo>(files, p => (IOHandler.ParseFileName(p) == valueFileName));
		}


		#endregion Methods
	}




}
