using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.ComponentModel;
using System.Net;

using MVVM;
using VKMusicSync.Model;
using VKMusicSync.ModelView;
using VKMusicSync.Handlers.Synchronize;
using vkontakte;
using VKMusicSync.Handlers;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Threading.Tasks;
using VKMusicSync.MVVM.Collections;
using System.Windows;

namespace VKMusicSync.ModelView
{
	public class SoundDownloaderMovelView : ListTabViewModel<Sound, SoundModelView>, IDataState
	{

		#region Private variables

		private List<Sound> modSoundsData;
		private ObservableCollection<SoundModelView> mvSounds = new ObservableCollection<SoundModelView>();


		//private List<Sound> cachedSounds = new List<Sound>();
		//private List<Sound> CachedSounds
		//{
		//		get
		//		{
		//				return cachedSounds;
		//		}
		//		set
		//		{
		//				cachedSounds = value;
		//		}
		//}

		#endregion

		#region Binding variables

		public bool LoadInfoFromLast
		{
			get
			{
				return Properties.Settings.Default.LoadInfoFromLastFm;
			}
			set
			{
				Properties.Settings.Default.LoadInfoFromLastFm = value;
				Properties.Settings.Default.Save();
			}
		}


		public string BackgroundPath
		{
			get
			{
				return Properties.Settings.Default.BackgroundPath;
			}
		}

		private string status;

		public List<Sound> SoundsData
		{
			get
			{
				return modSoundsData;
			}
			set
			{
				modSoundsData = value;
			}
		}

		public string Status
		{
			get
			{
				return status;
			}
			set
			{
				if (status == value)
					return;

				status = value;

				OnPropertyChanged("Status");
			}
		}

		// for old buttons

		//public double ProgressPercentage
		//{
		//		get
		//		{
		//				return progressPercentage;
		//		}
		//		set
		//		{
		//				progressPercentage = value;
		//				if (progressPercentage>=100)
		//				{
		//						ProgressVisibility = false;
		//				}
		//				else
		//				{
		//						if (progressVisibility==false)
		//								ProgressVisibility = true;
		//				}
		//				OnPropertyChanged("ProgressPercentage");
		//		}
		//}

		//private bool allChecked = true;
		//private string checkedText =  "Отменить все";

		//public string CheckedText
		//{
		//		get
		//		{
		//				return checkedText; 
		//		}
		//		set
		//		{
		//				checkedText = value;
		//				OnPropertyChanged("CheckedText");
		//		}
		//}

		//private string loadButtonText = "Синхронизация";

		//public string LoadButtonText
		//{
		//		get
		//		{
		//				return loadButtonText;
		//		}
		//		set
		//		{
		//				loadButtonText = value;
		//				OnPropertyChanged("LoadButtonText");
		//		}
		//}

		#endregion

		#region Click Commands

		private DelegateCommand checkAll;
		public ICommand CheckAll
		{
			get
			{
				if (checkAll == null)
				{
					checkAll = new DelegateCommand(OnCheckedAllClick, CheckIsLoaded);
				}
				return checkAll;
			}
			set
			{
				OnPropertyChanged("CheckAll");
			}

		}

		private bool CheckIsLoaded()
		{

			if (this.Items != null)
				if (this.Items.Count > 0)
					return true;

			return false;
		}


		private DelegateCommand downloadFiles;
		public ICommand DownloadFiles
		{
			get
			{
				if (downloadFiles == null)
				{
					downloadFiles = new DelegateCommand(StartSync, () => { return IsCanStartyngSync; });
				}
				return downloadFiles;
			}

		}

		private DelegateCommand cancelProcess;
		public ICommand CancelProcess
		{
			get
			{
				if (cancelProcess == null)
				{
					cancelProcess = new DelegateCommand(CancelSync, CheckIsLoaded);
				}
				return cancelProcess;
			}

		}

		private DelegateCommand sync;
		public ICommand SyncClick
		{
			get
			{
				if (sync == null)
				{
					sync = new DelegateCommand(OnUploadClick);
				}
				return sync;
			}

		}
		#endregion

		#region Constructor


		public SoundDownloaderMovelView()
			: base()
		{
			Header = Constants.Const.tbAudiosHeader;
			SoundsData = new List<Sound>();

			MainModelView.OnStateChanged += MainModelView_OnStateChanged;

		}

		private void MainModelView_OnStateChanged(VKApi.ConnectionState obj)
		{
			switch (obj)
			{
				case (VKApi.ConnectionState.Loaded):
					if (!IsFirstLoadDone || IsNeedFill)
						UpdateDataFromProfile(null);
					break;

				case (VKApi.ConnectionState.Failed):
					IsNeedFill = true;
					break;

				default:
					return;
			}
		}

		#endregion

		#region FormsActions

		private void OnSyncClick()
		{
			UpdateDataFromProfile(null);
		}

		private void OnShareClick()
		{
			//OnUploadClick();
			//return;
			vkontakte.CommandsGenerator.WallCommands.Post(
				+vkontakte.APIManager.Instance.AccessData.UserId,
				"VK Loader API test...my name :"
				+ vkontakte.APIManager.Instance.Profile.FullName,
				@"http://userserve-ak.last.fm/serve/500/97983211/MicroA.jpg",
				"",
				"");
			/*var t = Sounds;
			var info = LastFmHandler.Api.Track.GetInfo("Moby", "Porcelain");
			var res = LastFmHandler.Api.Artist.GetInfo("Moby");*/

		}

		private void OnCheckedAllClick()
		{

			if (allChecked)
			{
				allChecked = false;
				foreach (var value in Items)
					value.Checked = false;
				//this.CheckedText = "Выбрать все";

			}
			else
			{
				allChecked = true;
				foreach (var value in Items)
					value.Checked = true;
				//this.CheckedText = "Отменить все";
			}
			UpdateList();
		}

		public void UpdateList()
		{
			OnPropertyChanged("Items");
		}

		#endregion


		private bool IsCanStartyngSync
		{
			get
			{
				bool res = IsFirstLoadDone && IsNeedFill == false;

				res &= IsLoading == false;

				return res;
			}
		}

		#region Process API value to forms

		private void UpdateDataFromProfile(object obj)
		{
			IsLoading = true;
			var worker = new BackgroundWorker();
			//worker.WorkerSupportsCancellation = true;

			worker.DoWork += (p, arg) =>
			{

				Thread act2 = new Thread(() =>
				{
					Status = Constants.Status.LoadingTrackInfo;
					LoadAudioInfo();
					IsFirstLoadDone = true;
					IsNeedFill = false;
				});

				var manager = new AsyncTaskManager<Sound>();
				manager.Execute = new AsyncTaskManager<Sound>.ExecuteWork(PreloadDatFromLast);
				act2.IsBackground = true;
				act2.Start();
				act2.Join();

				Status = Constants.Status.LoadingTrackInfo;

				//manager.Start(SoundsData, Properties.Settings.Default.ThreadCountToUse);

				InitDone();

			};
			worker.RunWorkerAsync();
		}

		private void PreloadDatFromLast(Sound sound)
		{
			if (sound == null)
				return;

			try
			{
				Status = Constants.Status.LoadingTrackInfo + sound.artist;
				var artist = Handlers.LastFmHandler.Api.Artist.GetInfo(sound.artist);
				sound.authorPhotoPath = artist.Images[2].Value; // little spike 
				sound.similarArtists = artist.SimilarArtists.Select(el => el.Name).ToList<string>();

			}
			//catch (DotLastFm.Api.Rest.LastFmApiException ex)
			catch (Exception)
			{

			}
		}

		private void OnCommandLoading(Object sender, DownloadProgressChangedEventArgs e)
		{
			//this.ProgressPercentage = (double)Math.Abs(1 - e.ProgressPercentage);
		}

		private void InitDone()
		{
			Status = string.Format(Constants.Status.LoadingNTracksInfo, Items.Count);
			IsFirstLoadDone = true;
			Execute(() => IsLoading = false);
		}

		#endregion

		#region Items info load
		public void LoadAudioInfo()
		{
			SynhronizeAdapter<Sound> soundHandler;
			List<Sound> soundsData;

			soundHandler = new SynhronizeAdapter<Sound>(Properties.Settings.Default.DownloadFolderPath,
														Constants.Const.MP3,
														Properties.Settings.Default.ThreadCountToUse);

			//SoundHandler.OnDone += AdapterSyncFolderWithVKAsyncDone;
			//SoundHandler.OnProgress += AdapterSyncFolderWithVKAsyncOnProgress;


			soundHandler.OnReadDataInfoEvent += FilFromDiskItem;

			Func<Sound> Creator = () => { return new Sound(); };

			soundHandler.ComputeModList(Creator, DownloadProcces);

			soundsData = new List<Sound>();
			foreach (var item in soundHandler.ComputedFileList)
			{
				soundsData.Add(item);
			}

			base.ItemsData = soundsData;

			Execute(() => FillFromData(soundsData,
							p => { return new SoundModelView(p); }));

			SoundHandler = soundHandler;
			SoundsData = soundsData;
		}

		private void FilFromDiskItem(IDownnloadedData item)
		{
			var sound = item as Sound;
			if (sound != null)
				Handlers.TagReader.Read(item.PathWithFileName, sound);
		}

		private List<Sound> DownloadProcces()
		{
			int count_ = CommandsGenerator.AudioCommands.GetAudioCount(APIManager.Instance.API.UserId, false);

			if (count_ > 0)
			{
				CommandsGenerator.AudioCommands.OnCommandExecuting += OnCommandLoading;
				return CommandsGenerator.AudioCommands.GetAudioFromUser(APIManager.Instance.API.UserId, false, 0, count_);
			};
			return new List<Sound>();
		}

		#endregion

		#region Share

		public void ShareInfo()
		{
			//OnUploadClick();
			AudiosCommand profCommand = vkontakte.CommandsGenerator.AudioCommands.SendAudioToUserWall(APIManager.Instance.AccessData.UserId, 230);
			profCommand.ExecuteCommand();
		}

		#endregion

		#region Upload

		private void OnUploadClick()
		{
			System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
			dialog.ShowDialog();

			if (dialog.FileName.Count() > 0 && dialog.CheckPathExists)
			{
				AudioUploadedInfo info = CommandsGenerator.AudioCommands.GetUploadServer(Path.GetDirectoryName(dialog.FileName), Path.GetFileName(dialog.FileName));
				//AudioUploadedInfo info = CommandsGenerator.AudioCommands.GetUploadServer(@"D:\Musik\", @"myzuka.ru_08_nobody_but_me.mp3");
				string answer = CommandsGenerator.AudioCommands.SaveAudio(info);
			}


		}

		#endregion

		#region Sync audio

		private SynhronizeAdapter<Sound> SoundHandler;
		BackgroundWorker backgroundWorker;
		private bool allChecked;

		private void StartSync()
		{
			IsLoading = true;
			VKMusicSync.ModelView.SoundModelView.FreezeClick = true;
			backgroundWorker = new BackgroundWorker();
			//backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.WorkerSupportsCancellation = true;
			backgroundWorker.DoWork += SyncFolderWithVKAsync;
			backgroundWorker.RunWorkerAsync(SoundsData);
		}


		private void CancelSync()
		{
			SoundHandler.CancelDownloading();
			backgroundWorker.CancelAsync();
		}


		private void UploadItem(IDownnloadedData data)
		{
			UploadItem(data.Path, data.FileName + data.FileExtention);
		}

		private void UploadItem(string sourceFolderPath, string fileName)
		{
			AudioUploadedInfo info = CommandsGenerator.AudioCommands.GetUploadServer(sourceFolderPath, fileName);
			string answer = CommandsGenerator.AudioCommands.SaveAudio(info);
		}

		private void SyncFolderWithVKAsync(object sender, DoWorkEventArgs e)
		{
			SoundHandler = new SynhronizeAdapter<Sound>(Properties.Settings.Default.DownloadFolderPath,
			  "*.mp3", Properties.Settings.Default.ThreadCountToUse);

			SoundHandler.OnDone += AdapterSyncFolderWithVKAsyncDone;
			SoundHandler.OnProgress += AdapterSyncFolderWithVKAsyncOnProgress;
			SoundHandler.OnReadDataInfoEvent += new SynhronizerBase.HandleDataEvent(FilFromDiskItem);
			SoundHandler.OnUploadAction += new SynhronizerBase.HandleDataEvent(UploadItem);

			IEnumerable<SoundModelView> selected = Items.Where(p => p.Checked);

			SoundHandler.SyncFolderWithList<SoundModelView>(selected.ToList());
		}

		private void AdapterSyncFolderWithVKAsyncOnProgress(object sender, ProgressArgs e)
		{
			Status = SoundHandler.CountLoadedFiles + "/" + this.Items.Count;
			//this.ProgressPercentage = (e.ProgressPercentage * 100.0);

		}

		private void AdapterSyncFolderWithVKAsyncDone(object sender, ProgressArgs e)
		{
			Status = SoundHandler.CountLoadedFiles + "/" + this.Items.Count;
			//this.ProgressPercentage = 100;
			IOHandler.OpenPath(Properties.Settings.Default.DownloadFolderPath);
			IsLoading = false;
		}

		#endregion



		#region IDataState Members

		public bool IsNeedFill
		{
			get;
			set;
		}

		public bool IsFirstLoadDone
		{
			get;
			private set;
		}

		#endregion
	}
}
