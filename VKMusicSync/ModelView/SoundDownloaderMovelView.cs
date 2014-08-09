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

namespace VKMusicSync.ModelView
{
    class SoundDownloaderMovelView: ViewModelBase
    {

        public static SoundDownloaderMovelView Instance
        { get; private set; }

        #region Private variables

        private object lock0 = new object();
        private List<Sound> cachedSounds = new List<Sound>();
        private List<Sound> CachedSounds
        {
            get
            {
                lock(lock0)
                {
                    return cachedSounds;
                }
            }
            set
            {
                lock(lock0)
                {
                    cachedSounds = value;
                }
            }
        }

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


        private bool progressVisibility = false;
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

        private bool isSyncing = false;
        public bool IsSyncing
        {
            get
            {
                return isSyncing;
            }
            set
            {
                if (isSyncing!=value)
                {
                    isSyncing = value;
                    OnPropertyChanged("IsSyncing");
                }
            }
        }


        private object lock1 = new object();
        private ObservableCollection<SoundModelView> sound;
        public ObservableCollection<SoundModelView> Sounds { 
            get
            {
                lock(lock1)
                {
                    return sound;
                }
            }
            set
            {
                lock (lock1)
                {
                    sound = value;
                }
            }
        }


        private int tabSelectedIndex = 0;

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

        object lock2 = new object();

        private string status;
        public string Status
        {
            get
            {
                lock (lock2)
                {
                    return status; 
                }
            }
            set
            {
                lock(lock2)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public string UserFullName
        {
            get
            { return APIManager.Profile.ToString(); }
            set
            {
                OnPropertyChanged("UserFullName");
            }
        }

        //private System.Windows.Media.Imaging.BitmapFrame avatar;
        private string avatar;
       // public System.Windows.Media.Imaging.BitmapFrame Avatar
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

        private double progressPercentage = 0;
        public double ProgressPercentage
        {
            get
            {
                return progressPercentage;
            }
            set
            {
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

        private bool allChecked = true;
        private string checkedText =  "Отменить все";

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


        private string loadButtonText = "Синхронизация";

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
            if (this.Sounds != null)
                if (this.Sounds.Count > 0)
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
                    downloadFiles = new DelegateCommand(OnDownloadFiles, CheckIsLoaded);
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
        public SoundDownloaderMovelView(List<Sound> sounds)
        {
            Instance = this;
            SetSounds(sounds);
        }

        public SoundDownloaderMovelView()
        {
            Instance = this;
            CachedSounds = new List<Sound>();
            this.Sounds = new ObservableCollection<SoundModelView>();
        }

        private void SetSounds(List<Sound> sounds)
        {
            Sounds = new ObservableCollection<SoundModelView>(sounds.Select(s => new SoundModelView(s)));
        }
        #endregion

        #region FormsActions

        private void OnSyncClick()
        {
            UpdateDataFromProfile(null);
        }

        private void OnShareClick()
        {
            OnUploadClick();
            return;
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
            //System.Windows.Forms.MessageBox.Show("OnSyncClick");
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
            UpdateList();
        }

        public void UpdateList()
        {
            OnPropertyChanged("Sounds");
        }

        public void Window_Loaded()
        {
            OnAuthClick();
            UpdateDataFromProfile(null);
            //Thread thread = new Thread( new ParameterizedThreadStart( ));
            //thread.Start();
        }

        #endregion

        #region Process vk data to forms

        private void UpdateDataFromProfile(object obj)
        {

            var worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            
            worker.DoWork += (p,arg)=>{
                Thread act1 = new Thread(() =>
                {
                    Status = "Загрузка профиля";
                    LoadProfileInfo();
                });
                Thread act2 = new Thread(() =>
                {
                    Status = "Загрузка треков";
                    LoadAudioInfo();
                });

                Thread act5 = new Thread(() =>
                {
                    InitDone(null, null);
                });



                act1.IsBackground = true;
                act2.IsBackground = true;
                act5.IsBackground = true;
                act1.Start();
                act1.Join();
                act2.Start();
                act2.Join();

                var manager = new AsyncTaskManager<Sound>();
                manager.Execute = new AsyncTaskManager<Sound>.ExecuteWork(
                (sound) =>
                {
                    try
                    {
                        Status = "Загружаем информацию о треках..." + sound.artist;
                        var artist = Handlers.LastFmHandler.Api.Artist.GetInfo(sound.artist);
                        sound.authorPhotoPath = artist.Images[2].Value; // little spike 
                        sound.similarArtists = artist.SimilarArtists.Select( el => el.Name).ToList<string>();

                    }
                    //catch (DotLastFm.Api.Rest.LastFmApiException ex)
                    catch (Exception ex)
                    {

                    }
                });

                manager.Start(CachedSounds, Properties.Settings.Default.ThreadCountToUse);
                act5.Start();
                act5.Join();

            };
            worker.RunWorkerAsync();
            ProgressVisibility = true;
            //BackgroundWorker backgroundWorker = new BackgroundWorker();
            //backgroundWorker.DoWork += this.Init;
            //backgroundWorker.RunWorkerCompleted += this.InitDone;
            //backgroundWorker.RunWorkerAsync();
            //while(backgroundWorker.IsBusy)
            //{

            //}
        }

        //private void Init(object sender, DoWorkEventArgs e)
        //{
        //    Status = "Загрузка профиля";
        //    LoadProfileInfo();
        //    Status = "Загрузка треков";
        //    LoadAudioInfo();
        //    Status = "Загрузка информации о треках с Last.Fm";
        //    Handlers.ItemHelper.FillLastInfo(CachedSounds, LastFmHandler.Api);
        //    Status = "Размер файла";
        //    Handlers.ItemHelper.FillDataInfo(CachedSounds);
        //}

        private void OnCommandLoading(Object sender, DownloadProgressChangedEventArgs e)
        {
            this.ProgressPercentage = (double)Math.Abs(1 - e.ProgressPercentage);
        }

        private void InitDone(object sender, RunWorkerCompletedEventArgs e)
        {
            var s = CachedSounds;
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(
                ()=>{
                        this.Sounds.Clear();
                        for(int i=0; i<CachedSounds.Count; i++)
                            this.Sounds.Add(new SoundModelView(CachedSounds[i]));
                        Status = "Загружено информацию о " +this.Sounds.Count + " трэках";
                }),new object[]{});
            this.ProgressPercentage = 100;
            ProgressVisibility = false;
        }

        #endregion

        #region Information
        public void LoadAudioInfo()
        {
            int count = 4;//CommandsGenerator.AudioCommands.GetAudioCount(APIManager.vk.UserId, false);

            if (count > 0)
            {
                CommandsGenerator.AudioCommands.OnCommandExecuting += OnCommandLoading;
                CachedSounds = CommandsGenerator.AudioCommands.GetAudioFromUser(APIManager.vk.UserId, false, 0, count);
                //SetSounds(CachedSounds);
            }
        }

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
            
            /*AudiosCommand profCommand = vkontakte.CommandsGenerator.SendAudioToUserWall(APIManager.AccessData.UserId, 230);
            profCommand.ExecuteNonQuery();*/
        }

        #endregion

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

        #region Load audio

        
        private SynhronizeAdapter SoundHandler;
        BackgroundWorker backgroundWorker;

        private void OnDownloadFiles()
        {
                IsSyncing = true;
                VKMusicSync.ModelView.SoundModelView.FreezeClick = true;
                backgroundWorker = new BackgroundWorker();
                //backgroundWorker.WorkerReportsProgress = true;
                backgroundWorker.WorkerSupportsCancellation = true;
                backgroundWorker.DoWork += DoWork;
                backgroundWorker.RunWorkerAsync(CachedSounds);
        }

        private void CancelSync()
        {
            SoundHandler.CancelDownloading();
            backgroundWorker.CancelAsync();
        }

        private void OnProgress(object sender, ProgressArgs e)
        {
            Status = SoundHandler.CountLoadedFiles + "/" + this.Sounds.Count;  
            this.ProgressPercentage = (e.ProgressPercentage * 100.0);

        }

        private void OnDone(object sender, ProgressArgs e)
        {
            Status = SoundHandler.CountLoadedFiles + "/" + this.Sounds.Count;  
            this.ProgressPercentage = 100;
            IOHandler.OpenPath(Properties.Settings.Default.DownloadFolderPath);
            IsSyncing = false;

        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            SoundHandler = new SynhronizeAdapter(Properties.Settings.Default.DownloadFolderPath, Properties.Settings.Default.ThreadCountToUse);
            SoundHandler.OnDone += OnDone; 
            SoundHandler.OnProgress += OnProgress;
            IEnumerable<SoundModelView> selected = Sounds.Where(p => p.Checked);
            SoundHandler.SyncFolderWithList<SoundModelView>(selected.ToList());
        }
        #endregion


    }
}
