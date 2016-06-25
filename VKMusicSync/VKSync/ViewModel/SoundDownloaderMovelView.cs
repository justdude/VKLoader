using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.ComponentModel;
using System.Net;

using VKMusicSync.Model;
using VKMusicSync.ModelView;
using VKMusicSync.Handlers.Synchronize;
using VKLib;
using VKMusicSync.Handlers;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using VKMusicSync.Handlers.CachedData;
using DotLastFm.Models;
using VKLib.Model;
using VKMusicSync.Messages;
using MIP.Commands;
using MVVM;
using VkDay.vkontakte;
using VKMusicSync.Handlers.IoC;
using Microsoft.Practices.Unity;


namespace VKMusicSync.VKSync.ViewModel
{
	public class SoundDownloaderMovelView : ListTabViewModel<Sound, SoundModelView>, IDataState
	{

		#region Private variables

		private List<Sound> modSoundsData;
		private ObservableCollection<SoundModelView> mvSounds = new ObservableCollection<SoundModelView>();


		private readonly DelegateCommand modCheckAll;
		private readonly DelegateCommand modDownloadFiles;
		private readonly DelegateCommand modCancelProcess;
		private readonly DelegateCommand modSyncClick;
		private readonly DelegateCommand modCloseTabCommand;

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

				base.RaisePropertyChanged(() => this.Status);
			}
		}

		public bool IsSyncing
		{
			get
			{
				return mvIsSyncing;
			}
			set
			{
				if (mvIsSyncing == value)
					return;

				mvIsSyncing = value;

				RaisePropertyChanged<bool>(() => IsSyncing);
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

		#region Ctr

		public SoundDownloaderMovelView()
			: base()
		{
			Header = Constants.Const.tbAudiosHeader;
			SoundsData = new List<Sound>();

			modCheckAll = new DelegateCommand(OnCheckedAllClick, CanCheckAll);
			modDownloadFiles = new DelegateCommand(DownloadFiles, InCanStartSync);
			modCancelProcess = new DelegateCommand(CancelSync, CanCansel);
			modSyncClick = new DelegateCommand(OnUploadClick);
			modCloseTabCommand = new DelegateCommand(OnCloseTab, CanCloseTab);
		}

		#region Click Commands

		public override ICommand CloseTab
		{
			get
			{
				return modCloseTabCommand;
			}
		}

		public ICommand CheckAll
		{
			get
			{
				return modCheckAll;
			}
		}

		public ICommand DownloadFilesClick
		{
			get
			{
				return modDownloadFiles;
			}

		}

		public ICommand CancelProcess
		{
			get
			{
				return modCancelProcess;
			}
		}

		public ICommand SyncClick
		{
			get
			{
				return modSyncClick;
			}

		}
		#endregion

		#region Checkers

		private bool CanCheckAll()
		{

			if (this.Items == null)
				return false;

			if (this.Items.Count > 0 && IsLoading)
				return true;

			return false;
		}

		private bool InCanStartSync()
		{
			return IsCanStartyngSync && !IsSyncing;// && !IsLoading;
		}

		private bool CanCansel()
		{
			if (IsSyncing)
				return true;

			return false;// && IsLoading;
		}

		private bool CanCloseTab()
		{
			return false;
		}

		#endregion


		private void OnCloseTab()
		{
			CleanAndClose();
		}

		private void MainModelView_OnStateChanged(VKApi.ConnectionState obj)
		{
			switch (obj)
			{
				case (VKApi.ConnectionState.Loaded):
					if (!IsFirstLoadDone || IsNeedFill)
					{
						Execute(() =>
						{
							RaisePropertiesChanged();
							RefreshCommands();
						});
						UpdateDataFromProfile(null);
					}
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
			VKLib.CommandsGenerator.WallCommands.Post(
				+vkWrapper.UserProfile.uid,
				string.Format("VK Loader API test...my name :{0}", vkWrapper.UserProfile.FullName),
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
					value.Checked = allChecked;
				//this.CheckedText = "Выбрать все";

			}
			else
			{
				allChecked = true;
				foreach (var value in Items)
					value.Checked = allChecked;
				//this.CheckedText = "Отменить все";
			}
			RefreshCommands();

			//UpdateList();
		}

		public void UpdateList()
		{
			base.RaisePropertyChanged(() => Items);
		}

		#endregion

		private bool IsCanStartyngSync
		{
			get
			{
				return IsFirstLoadDone && !IsNeedFill;
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

				//Thread act2 = new Thread(() =>
				//{
				Status = Constants.Status.LoadingTrackInfo;
				LoadAudioInfo();
				IsFirstLoadDone = true;
				//	IsNeedFill = false;
				//});


				//act2.IsBackground = true;
				//act2.Start();
				//act2.Join();
				//InitDone();

			};
			worker.RunWorkerAsync();

			worker.RunWorkerCompleted += (s, e) =>
				{
					var manager = new AsyncTaskManager<Sound>(PreloadDatFromLast);
					Status = Constants.Status.LoadingTrackInfo;

					manager.ProcessAsync(SoundsData, Properties.Settings.Default.ThreadCountToUse);
					manager.AllDone.WaitOne();
					InitDone();
				};
		}

		private void PreloadDatFromLast(Sound sound)
		{
			if (sound == null)
				return;

			try
			{
				//ArtistWithDetails artist = Cache.Get(sound.artist) as ArtistWithDetails;
				//Cache.AddIfNotExist(sound.artist, artist);
				ArtistWithDetails artist = null;

				if (artist == null)
					artist = Handlers.LastFmHandler.Api.Artist.GetInfo(sound.artist);

				Execute(() =>
				{
					Status = Constants.Status.LoadingTrackInfo + sound.artist;
					sound.authorPhotoPath = artist.Images[2].Value; // little spike 
					sound.similarArtists = artist.SimilarArtists.Select(el => el.Name).ToList<string>();
				});
			}
			//catch (DotLastFm.Api.Rest.LastFmApiException ex)
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
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

			Execute(() => FillFromData(p => { return new SoundModelView(p); }));

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
			int count_ = CommandsGenerator.AudioCommands.GetAudioCount(vkWrapper.UserProfile.uid, false);

			if (count_ > 0)
			{
				CommandsGenerator.AudioCommands.OnCommandExecuting += OnCommandLoading;

				List<SoundBase> sounds = CommandsGenerator.AudioCommands.GetAudioFromUser(vkWrapper.UserProfile.uid, false, 0, count_);

				if (sounds == null)
					return new List<Sound>();


				return sounds.Select(p => new Sound(p)).ToList();

			};
			return new List<Sound>();
		}

		#endregion

		#region Share

		public void ShareInfo()
		{
			//OnUploadClick();
			AudiosCommand profCommand = VKLib.CommandsGenerator.AudioCommands.SendAudioToUserWall(vkWrapper.UserProfile.uid, 230);
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
		private bool mvIsSyncing;
		private IVkWrapper vkWrapper;

		private void DownloadFiles()
		{
			IsLoading = true;
			IsSyncing = true;

			VKMusicSync.ModelView.SoundModelView.FreezeClick = true;
			backgroundWorker = new BackgroundWorker();
			//backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.WorkerSupportsCancellation = true;
			backgroundWorker.DoWork += SyncFolderWithVKAsync;
			backgroundWorker.RunWorkerAsync(SoundsData);
		}


		private void CancelSync()
		{
			try
			{
				SoundHandler.CancelDownloading();
				backgroundWorker.CancelAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
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

			BeginExecute(() =>
				{
					IsLoading = false;
					IsSyncing = false;
					RaisePropertiesChanged();
					RefreshCommands();
				});
		}

		#endregion

		protected override void OnTokenChanged()
		{
			MainModelView.OnStateChanged += MainModelView_OnStateChanged;
			MessengerInstance.Register<VkLoaded>(this, OnVkLoaded);

			vkWrapper = Unity.Instance.Resolve<IVkWrapper>();

			base.OnTokenChanged();
		}

		private void OnVkLoaded(VkLoaded obj)
		{
			RaisePropertiesChanged();
			RefreshCommands();
		}

		protected override void OnCleanup()
		{
			MainModelView.OnStateChanged -= MainModelView_OnStateChanged;

			try
			{
				if (SoundHandler != null)
					SoundHandler.CancelDownloading();

				CancelSync();
			}
			catch (Exception)
			{ }

			base.OnCleanup();
		}


		#region ViewModel overrides

		private void RaisePropertiesChanged()
		{
			RaisePropertyChanged<bool>(() => IsSyncing);
		}

		public override void RefreshCommands()
		{
			modCheckAll.RaiseCanExecuteChanged();
			modDownloadFiles.RaiseCanExecuteChanged();
			modCancelProcess.RaiseCanExecuteChanged();
			modSyncClick.RaiseCanExecuteChanged();
			modCloseTabCommand.RaiseCanExecuteChanged();
			base.RefreshCommands();
		}

		public override void Cleanup()
		{
			base.Cleanup();
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
