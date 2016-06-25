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
using Microsoft.Practices.Unity;
using VKMusicSync.Messages;
using MIP.MVVM;
using MIP.Commands;
using VkDay.vkontakte;
using VKLib.vkontakte;
using VKMusicSync.Handlers.IoC;
using VKMusicSync.UserInfo.ViewModel;
using VKMusicSync.VKSync.ViewModel;

namespace VKMusicSync.ModelView
{
	public class MainModelView : AdwancedViewModelBase, IDataState
	{

		#region Fields

		private bool progressVisibility = false;
		private bool isSyncing = false;
		private string status;
		private int tabSelectedIndex = 0;
		private string avatar = Constants.Const.DefaultAvatar;

		#endregion

		#region Connection state

		public static event Action<VKApi.ConnectionState> OnStateChanged;

		private static void ChangeConnectionState(VKApi.ConnectionState state)
		{
			if (OnStateChanged == null)
				return;

			OnStateChanged(state);
		}

		#endregion

		#region Properties

		public ObservableCollection<TabModelView> Tabs { get; private set; }

		public bool LoadInfoFromLast
		{
			get
			{
				return Properties.Settings.Default.LoadInfoFromLastFm;
			}
			set
			{
				if (Properties.Settings.Default.LoadInfoFromLastFm == value)
					return;

				Properties.Settings.Default.LoadInfoFromLastFm = value;
				Properties.Settings.Default.Save();

				RaisePropertyChanged<bool>(() => LoadInfoFromLast);
			}
		}


		public string BackgroundPath
		{
			get
			{
				return Properties.Settings.Default.BackgroundPath;
			}
		}

		public bool ProgressVisibility
		{
			get
			{
				return progressVisibility;
			}
			set
			{
				progressVisibility = value;
				RaisePropertyChanged<bool>(() => ProgressVisibility);
			}
		}

		public bool IsSyncing
		{
			get
			{
				return isSyncing;
			}
			set
			{
				if (isSyncing == value)
					return;

				isSyncing = value;
				RaisePropertyChanged<bool>(() => IsSyncing);
			}
		}

		public int TabSelectedIndex
		{
			get
			{ return tabSelectedIndex; }
			set
			{
				//System.Windows.Forms.MessageBox.Show(tabSelectedIndex.ToString());
				tabSelectedIndex = value;
				RaisePropertyChanged<int>(() => TabSelectedIndex);
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

				RaisePropertyChanged<string>(() => Status);
			}
		}

		public string UserFullName
		{
			get
			{
				return vkWrapper.IsUserLoaded
					? vkWrapper.UserProfile.ToString()
					: string.Empty;
			}
		}


		public string Avatar
		{
			get
			{
				return avatar;
			}
			set
			{
				if (avatar == value)
					return;

				avatar = value;

				RaisePropertyChanged<string>(() => Avatar);
			}
		}

		#endregion

		#region Click Commands

		private readonly DelegateCommand modSettingsCLickCommand;
		private readonly DelegateCommand modShareClickCommand;
		private IVkWrapper vkWrapper;

		public ICommand SettingsClick
		{
			get
			{
				return modSettingsCLickCommand;
			}

		}

		public ICommand ShareClick
		{
			get
			{
				return modShareClickCommand;
			}

		}
		#endregion

		#region Ctr.

		public MainModelView()
		{
			modSettingsCLickCommand = new DelegateCommand(OnSettingsClick);
			modShareClickCommand = new DelegateCommand(OnShareClick, CanShareClick);

			Tabs = new ObservableCollection<TabModelView>();

			Unity.Instance.RegisterType<IVkWrapper, VkWrapper>();

			vkWrapper = Unity.Instance.Resolve<IVkWrapper>();
		}

		#endregion

		private bool CanShareClick()
		{
			return IsEnabled;
		}



		#region Events listen

		void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			Thread.Sleep(1000 * 1);
			Execute(vkWrapper.ShowAutorizationWindow);
		}

		void API_OnConnectionStateChanged(object sender, VKLib.VKApi.ConnectionState e)
		{
			if (e != VKLib.VKApi.ConnectionState.Loaded)
				return;

			ThreadPool.QueueUserWorkItem((p) =>
			{
				BeginExecute(() => IsLoading = true);

				vkWrapper.InitUser();

				BeginExecute(() => IsLoading = false);
			});
		}

		void vk_OnStateChanged(object sender, VKLib.VKApi.ConnectionState e)
		{
			ChangeConnectionState(e);

			if (e != VKLib.VKApi.ConnectionState.Loaded)
				return;

			UpdateDataFromProfile();
		}

		#endregion

		#region FormsActions

		private void OnShareClick()
		{
			if (!vkWrapper.IsUserLoaded)
				return;

			VKLib.CommandsGenerator.WallCommands.Post(
					+vkWrapper.UserProfile.uid,
					string.Format("VK Loader API test...my name :{0}", vkWrapper.UserProfile.FullName),
					@"http://userserve-ak.last.fm/serve/500/97983211/MicroA.jpg",
					string.Empty, string.Empty);
			/*var t = Sounds;
			var info = LastFmHandler.Api.Track.GetInfo("Moby", "Porcelain");
			var res = LastFmHandler.Api.Artist.GetInfo("Moby");*/

		}

		private void OnSettingsClick()
		{
			var form = new VKMusicSync.View.Settings();
			form.ShowDialog();
		}

		#endregion

		#region Process API data

		private void UpdateDataFromProfile()
		{
			Status = Constants.Status.ProfileLoading;

			LoadProfileInfo();

			Status = string.Empty;
			Execute(() =>
			{
				IsFirstLoadDone = true;
			});
		}

		#endregion

		#region UserProfile

		public void LoadProfileInfo()
		{
			if (!vkWrapper.IsUserLoaded)
				return;

			string path = vkWrapper.UserProfile.GetMaxPhotoSize();

			if (path != string.Empty)
				Execute(() => Avatar = path);

			Execute(() => Refresh());
		}
		#endregion

		#region Share

		public void ShareInfo()
		{
			/*AudiosCommand profCommand = VKLib.CommandsGenerator.SendAudioToUserWall(APIManager.AccessData.UserId, 230);
			profCommand.ExecuteNonQuery();*/
		}

		#endregion

		#region ViewModel overrides

		private void RaisePropertyesChaned()
		{
			RaisePropertyChanged<string>(() => BackgroundPath);
			RaisePropertyChanged<bool>(() => LoadInfoFromLast);
			RaisePropertyChanged<bool>(() => ProgressVisibility);
			RaisePropertyChanged<bool>(() => IsSyncing);
			RaisePropertyChanged<int>(() => TabSelectedIndex);
			RaisePropertyChanged<string>(() => Status);
			RaisePropertyChanged<string>(() => Avatar);
			RaisePropertyChanged<string>(() => UserFullName);
		}

		public override void RefreshCommands()
		{
			modSettingsCLickCommand.RaiseCanExecuteChanged();
			modShareClickCommand.RaiseCanExecuteChanged();
			base.RefreshCommands();
		}

		protected override void RefreshPrivate()
		{
			RaisePropertyesChaned();
			RefreshCommands();
			base.RefreshPrivate();
		}

		protected override void OnTokenChanged()
		{
			vkWrapper.UserLoadedAction += vk_OnStateChanged;
			vkWrapper.AutorizedAction += API_OnConnectionStateChanged;

			var tab = new SoundDownloaderMovelView() { ParentToken = Token };
			var tab2 = new UserInfoViewModel() { ParentToken = Token };

			Tabs.Add(tab2);
			Tabs.Add(tab);

			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += worker_DoWork;
			worker.RunWorkerCompleted += worker_RunWorkerCompleted;
			worker.RunWorkerAsync();

			base.OnTokenChanged();
		}

		void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MessengerInstance.Send<VkLoaded>(new VkLoaded());
		}

		protected override void OnCleanup()
		{
			foreach (var item in Tabs)
			{
				item.Clean();
			}
			Tabs.Clear();

			vkWrapper.AutorizedAction -= vk_OnStateChanged;
			vkWrapper.UserLoadedAction -= API_OnConnectionStateChanged;

			base.OnCleanup();
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
