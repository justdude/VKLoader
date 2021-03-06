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
using VKSyncMusic.Model;
using VKSyncMusic.ModelView;
using VKSyncMusic.Handlers.Synchronize;
using vkontakte;
using VKSyncMusic.Handlers;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Threading.Tasks;
using VKSyncMusic.MVVM.Collections;
using System.Windows;
using VKSyncMusic.Interfaces;
using VKSyncMusic.SoundBuilders;
using VKSyncMusic.ModelView.Tabs;
using System.Windows.Controls;
using VKSyncMusic.View;


namespace VKSyncMusic.ModelView
{
    class MainModelView: ViewModelBase
    {

        public static MainModelView Instance
        { get; private set; }

        #region Fields
				private string checkedText = "Отменить все";
				private bool allChecked = true;
				private string loadButtonText = "Синхронизация";
				private double progressPercentage = 0;
				private string avatar = @"http://upload.wikimedia.org/wikipedia/commons/thumb/2/2a/Flag_of_None.svg/225px-Flag_of_None.svg.png";
				private string status;
				private int tabSelectedIndex = 0;
				private bool progressVisibility = false;
				private bool isSyncing = false;
				private object lock1 = new object();
				private List<Sound> modSoundsData;
				private ObservableCollection<SoundModelView> mvSounds = new ObservableCollection<SoundModelView>();

        #endregion

        #region Binding Properties


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
							if (progressVisibility == value)
								return;

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
							if (isSyncing == value)
								return;

              isSyncing = value;

              OnPropertyChanged("IsSyncing");
            }
        }
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
        public ObservableCollection<SoundModelView> Sounds 
        { 
            get
            {
                return mvSounds;
            }
            set
            {
                mvSounds = value;
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
                return APIManager.Profile.ToString(); }
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
        public double ProgressPercentage
        {
            get
            {
                return progressPercentage;
            }
            set
            {
							if (progressPercentage == value)
								return;

                progressPercentage = value;

                if (progressPercentage>=100)
                {
                    ProgressVisibility = false;
                }
                else
                {
                    if (progressVisibility==false)
                        ProgressVisibility = true;
                }

                OnPropertyChanged("ProgressPercentage");
            }
        }
        public string CheckedText
        {
            get
            {
                return checkedText; 
            }
            set
            {
                checkedText = value;
                OnPropertyChanged("CheckedText");
            }
        }

        public string LoadButtonText
        {
            get
            {
                return loadButtonText;
            }
            set
            {
                loadButtonText = value;
                OnPropertyChanged("LoadButtonText");
            }
        }

        #endregion

				public ObservableCollection<TabItem> TabItems { get; private set; }
				public int TabSelectedIndex
				{
					get
					{
						return tabSelectedIndex;
					}
					set
					{
						//System.Windows.Forms.MessageBox.Show(tabSelectedIndex.ToString());
						tabSelectedIndex = value;
						OnPropertyChanged("TabSelectedIndex");
					}
				}

        #region Click Commands
				private DelegateCommand downloadFiles;
				private DelegateCommand cancelProcess;
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
            return true;
        }


        
        public ICommand DownloadFiles
        {
            get
            {
                if (downloadFiles == null)
                {
                    downloadFiles = new DelegateCommand(OnDownloadFiles, CheckIsLoaded);
                }
                return downloadFiles;
            }

        }

				private void OnDownloadFiles()
				{
					throw new NotImplementedException();
				}

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

				private void CancelSync()
				{
					throw new NotImplementedException();
				}

        

        private DelegateCommand settings;
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

        private DelegateCommand shareClick;
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

        public MainModelView()
        {

					TabItems = new ObservableCollection<System.Windows.Controls.TabItem>();
					InitialTabs = WPFExtension.GetConstantsContentElement("collTabs");


					//new SoundListModelView();
          Instance = this;
					Window_Loaded();
        }

				private void AddTab(Constants.TabTypes type)
				{
					var tabViewModel = new TabModelView(type);
					TabItem first = InitialTabs.Where(p => p.Tag.ToString() == type.ToString()).FirstOrDefault();

					//string targ = type.ToString();
					//foreach(var item in InitialTabs)
					//{
					//	if ((item.Tag as string) == targ)
					//		first = item;
					//		break;
					//}

					//if (first == null)
					//	return;

					TabItem newTabItem = new TabItem();
					newTabItem.Style = first.Style;
					newTabItem.Template = first.Template;

					newTabItem.SetData<TabModelView>(tabViewModel);
					newTabItem.SetDataToTag(tabViewModel.TypeName);

					TabItems.Add(first);
				}


        #endregion

        #region FormsActions

        private void OnSyncClick()
        {
        }

				//public readonly Dictionary<string, SoundProccesorBuilder> Connections = new Dictionary<string, SoundProccesorBuilder>()
				//{
				//	{	"VKAUDIO", new VkListProccesorBuilder()},
				//	{	"VKRECOMMENDATION", new VkListProccesorBuilder()},
				//	{	"VKAUDIO", new VkListProccesorBuilder()}
				//};

				private void OnTabChanged(ISoundListModelView modelView, string type)
				{
					if (modelView == null)
						return;

					var processor = GetBuilderByType(type);

					if (processor == null)
						return;

					modelView.Processor = processor;
					modelView.UpdateList();

				}

				private SynhronizeProcessor<Sound> GetBuilderByType(string type)
				{
					SoundProccesorBuilder builder = new VkListProccesorBuilder();
					Processor proccesor = new Processor(builder);

					proccesor.Build();
					return proccesor.GetResult();
				}

        private void OnShareClick()
        {
            OnUploadClick();

						//vkontakte.CommandsGenerator.WallCommands.Post(
						//		+vkontakte.APIManager.AccessData.UserId,
						//		"VK Loader API test...my name :"
						//		+ vkontakte.APIManager.Profile.FullName,
						//		@"http://userserve-ak.last.fm/serve/500/97983211/MicroA.jpg",
						//		"",
						//		"");
            /*var t = Target;
            var info = LastFmHandler.Api.Track.GetInfo("Moby", "Porcelain");
            var res = LastFmHandler.Api.Artist.GetInfo("Moby");*/

        }

        private void OnSettingsClick()
        {
            var form = new VKSyncMusic.View.Settings();
            form.ShowDialog();
        }

        private void OnAuthClick()
        {
            var authWindow = new Auth();
            authWindow.ShowDialog();
        }


        private void OnCheckedAllClick()
        {
            
            if (allChecked)
            {
                allChecked = false;
                foreach (var value in Sounds)
                    value.Checked = false;
                this.CheckedText = "Выбрать все";
                
            }
            else
            {
                allChecked = true;
                foreach (var value in Sounds)
                    value.Checked = true;
                this.CheckedText = "Отменить все";
            }

        }


        public void Window_Loaded()
        {
            OnAuthClick();
            var task = UpdateDataFromProfile();
						task.Start();

						AddTab(Constants.TabTypes.VkAudio);
						AddTab(Constants.TabTypes.VkPopular);

						foreach(var item in TabItems)
						{

							var tabDataContext = item.GetContent<View.AudioList>();
							var modelView = tabDataContext.GetDataContext<SoundListModelView>() as ISoundListModelView;

							if (modelView == null)
								continue;

							var process = SynhronizeProcessor<Sound>.Create();
							modelView.Processor = process;
							modelView.UpdateList();
						}
        }

        #endregion

        #region Process vk value to forms

        private Task UpdateDataFromProfile()
        {
					Task tast = new Task(()=> LoadProfileInfo());
					ProgressVisibility = true;
					
					return tast;
        }

        private void OnCommandLoading(Object sender, DownloadProgressChangedEventArgs e)
        {
            this.ProgressPercentage = (double)Math.Abs(1 - e.ProgressPercentage);
        }

        private void InitDone(object sender, RunWorkerCompletedEventArgs e)
        {
            Status = "Загружено информацию о " + this.Sounds.Count + " трэках";
            this.ProgressPercentage = 100;
            ProgressVisibility = false;
        }

        #endregion


        #region Profile

        public void LoadProfileInfo()
        {
            APIManager.Profile = CommandsGenerator.ProfileCommands.GetUser(APIManager.AccessData.UserId);
            var paths = (new List<string>() { APIManager.Profile.photo, APIManager.Profile.photoMedium, APIManager.Profile.photoBig });
            var leng = paths.Max(p => p.Length);
            string path = paths.FirstOrDefault(p => p.Length == leng);
            if (path != string.Empty)
                Avatar = path;

            this.UserFullName = APIManager.Profile.last_name;
        }
        #endregion

        #region Share

        public void ShareInfo()
        {
            OnUploadClick();
            /*AudiosCommand profCommand = vkontakte.CommandsGenerator.SendAudioToUserWall(APIManager.AccessData.UserId, 230);
            profCommand.ExecuteNonQuery();*/
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

				//#region Sync audio

				//private SynhronizeProcessor<Sound> SoundHandler;
				//BackgroundWorker backgroundWorker;

				//private void OnDownloadFiles()
				//{
				//				IsSyncing = true;
				//				VKSyncMusic.ModelView.SoundModelView.FreezeClick = true;
				//}


				//private void CancelSync()
				//{
				//		SoundHandler.CancelDownloading();
				//		backgroundWorker.CancelAsync();
				//}

				//#endregion



				public TabItem[] InitialTabs { get; set; }
		}
}
