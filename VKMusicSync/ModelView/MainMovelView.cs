﻿using System;
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
	public class MainMovelView : ViewModelBase
	{

		#region Fields

		private bool progressVisibility = false;
		private bool isSyncing = false;
		private string status;
		private int tabSelectedIndex = 0;
		private string avatar = Constants.Constants.DefaultAvatar;

		#endregion

		#region Properties

		public ObservableCollection<TabModelView> Tabs { get; private set;}

		public bool IsInited { get; set; }

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
				if (APIManager.Profile == null)
					return "";
				return APIManager.Profile.ToString();
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

		public MainMovelView()
		{
			FIll();
			Tabs = new ObservableCollection<TabModelView>();
			var tab = new SoundDownloaderMovelView();
			Tabs.Add(tab);
			
		}

		#endregion

		#region FormsActions

		private void OnShareClick()
		{
			vkontakte.CommandsGenerator.WallCommands.Post(
					+vkontakte.APIManager.AccessData.UserId,
					"VK Loader API test...my name :"
					+ vkontakte.APIManager.Profile.FullName,
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

		private void OnAuthClick()
		{
			var authWindow = new Auth();
			authWindow.ShowDialog();
		}

		public void FIll()
		{
			OnAuthClick();
			UpdateDataFromProfile(null);
		}

		#endregion

		#region Process vk data

		private void UpdateDataFromProfile(object obj)
		{

			var worker = new BackgroundWorker();
			worker.WorkerSupportsCancellation = true;

			worker.DoWork += (p, arg) =>
			{
					Status = "Загрузка профиля";
					LoadProfileInfo();
			};

		}

		#endregion
		
		#region Profile

		public void LoadProfileInfo()
		{
			APIManager.Profile = CommandsGenerator.ProfileCommands.GetUser(APIManager.AccessData.UserId);

			var paths = (new List<string>() 
						{ 
							APIManager.Profile.photo, 
							APIManager.Profile.photoMedium, 
							APIManager.Profile.photoBig });

			var leng = paths.Max(p => p.Length);

			string path = paths.FirstOrDefault(p => p.Length == leng);

			if (path != string.Empty)
				Execute(()=> Avatar = path);

			Execute(()=> UserFullName = APIManager.Profile.last_name);
			IsInited = true;
		}
		#endregion

		#region Share

		public void ShareInfo()
		{
			/*AudiosCommand profCommand = vkontakte.CommandsGenerator.SendAudioToUserWall(APIManager.AccessData.UserId, 230);
			profCommand.ExecuteNonQuery();*/
		}

		#endregion

	}
}
