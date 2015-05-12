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
using VkDay;
using VKMusicSync.Handlers;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Threading.Tasks;
using VKMusicSync.MVVM.Collections;
using System.Windows;
using VkDay;

namespace VKMusicSync.ModelView
{
	public class MainModelView : ViewModelBase, IDataState
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

		public bool ProgressVisibility
		{
			get
			{
				return progressVisibility;
			}
			set
			{
				progressVisibility = value;
				OnPropertyChanged("ProgressVisibility");
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
				if (isSyncing != value)
				{
					isSyncing = value;
					OnPropertyChanged("IsSyncing");
				}
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
				OnPropertyChanged("TabSelectedIndex");
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

		public string UserFullName
		{
			get
			{
				if (APIManager.Instance.Profile == null)
					return "";
				return APIManager.Instance.Profile.ToString();
			}
			set
			{
				OnPropertyChanged("UserFullName");
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
				avatar = value;
				OnPropertyChanged("Avatar");
			}
		}

		#endregion

		#region Click Commands

		private DelegateCommand settings;
		private DelegateCommand shareClick;

		public ICommand SettingsClick
		{
			get
			{
				if (settings == null)
				{
					settings = new DelegateCommand(OnSettingsClick);
				}
				return settings;
			}

		}

		public ICommand ShareClick
		{
			get
			{
				if (shareClick == null)
				{
					shareClick = new DelegateCommand(OnShareClick);
				}
				return shareClick;
			}

		}
		#endregion

		#region Constructor

		public MainModelView()
		{
			Tabs = new ObservableCollection<TabModelView>();
			var tab = new SoundDownloaderMovelView();
			Tabs.Add(tab);

			VkDay.APIManager.Instance.OnUserLoaded += vk_OnStateChanged;
			VkDay.APIManager.Instance.API.OnConnectionStateChanged += API_OnConnectionStateChanged;
			APIManager.Instance.Connect();

		}

		#endregion

		#region Events listen

		void API_OnConnectionStateChanged(VkDay.VKApi.ConnectionState obj)
		{
			if (obj != VkDay.VKApi.ConnectionState.Loaded)
				return;

			ThreadPool.QueueUserWorkItem((p) =>
			{
				BeginExecute(() => IsLoading = true);
				APIManager.Instance.InitUser();
				BeginExecute(() => IsLoading = false);
			});
		}

		void vk_OnStateChanged(VkDay.VKApi.ConnectionState obj)
		{
			ChangeConnectionState(obj);

			if (obj != VkDay.VKApi.ConnectionState.Loaded)
				return;

			UpdateDataFromProfile();
		}

		#endregion

		#region FormsActions

		private void OnShareClick()
		{
			VkDay.CommandsGenerator.WallCommands.Post(
					+VkDay.APIManager.Instance.AccessData.UserId,
					"VK Loader API test...my name :"
					+ VkDay.APIManager.Instance.Profile.FullName,
					@"http://userserve-ak.last.fm/serve/500/97983211/MicroA.jpg",
					"",
					"");
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
			Execute(() => { IsFirstLoadDone = true; });
		}

		#endregion

		#region Profile

		public void LoadProfileInfo()
		{
			string path = APIManager.Instance.Profile.GetMaxPhotoSize();

			if (path != string.Empty)
				Execute(() => Avatar = path);

			Execute(() => UserFullName = APIManager.Instance.Profile.last_name);
		}
		#endregion

		#region Share

		public void ShareInfo()
		{
			/*AudiosCommand profCommand = VkDay.CommandsGenerator.SendAudioToUserWall(APIManager.AccessData.UserId, 230);
			profCommand.ExecuteNonQuery();*/
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
