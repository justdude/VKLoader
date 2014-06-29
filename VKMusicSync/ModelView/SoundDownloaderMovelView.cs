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

namespace VKMusicSync.ModelView
{
    class SoundDownloaderMovelView: ViewModelBase
    {

        public static SoundDownloaderMovelView Instance
        { get; private set; }

        #region Private variables

        private List<Sound> sounds = new List<Sound>();

        #endregion

        #region Binding variables

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


        public ObservableCollection<SoundModelView> Sounds { get; set; }


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

        private string status;
        public string Status
        {
            get
            { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
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
                    checkAll = new DelegateCommand(OnCheckedAllClick);
                }
                return checkAll;
            }
            set
            {
                OnPropertyChanged("CheckAll");
            }

        }

        private DelegateCommand downloadFiles;
        public ICommand DownloadFiles
        {
            get
            {
                if (downloadFiles == null)
                {
                    downloadFiles = new DelegateCommand(OnDownloadFiles);
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
                    cancelProcess = new DelegateCommand(CancelSync);
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
            sounds = new List<Sound>();
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
            UpdateDataFromProfile();
        }

        private void OnShareClick()
        {

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
            UpdateDataFromProfile();
        }

        #endregion

        #region Process vk data to forms

        private void UpdateDataFromProfile()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += this.Init;
            backgroundWorker.RunWorkerCompleted += this.InitDone;
            backgroundWorker.RunWorkerAsync();
        }

        private void Init(object sender, DoWorkEventArgs e)
        {
            LoadProfileInfo();
            LoadAudioInfo();
        }

        private void OnCommandLoading(Object sender, DownloadProgressChangedEventArgs e)
        {
            this.ProgressPercentage = (double)Math.Abs(1 - e.ProgressPercentage);
        }

        private void InitDone(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Sounds.Clear();
            for(int i=0; i<sounds.Count; i++)
                this.Sounds.Add(new SoundModelView(sounds[i]));
            SetSounds(sounds);
            this.ProgressPercentage = 100;
            Status = "" + this.Sounds.Count;
        }

        #endregion

        #region Information
        public void LoadAudioInfo()
        {
            int count = int.Parse(APIManager.vk.GetAudioCountFromUser(APIManager.vk.UserId,
                                                          false).SelectSingleNode("response")
                                                                .InnerText);

            if (count > 0)
            {
                AudioCommand command = vkontakte.CommandsGenerator.GetAudioFromUser(APIManager.vk.UserId, false, 0, count);
                command.OnCommandExecuting += OnCommandLoading;
                sounds = command.ExecuteForList();
            }
        }

        public void LoadProfileInfo()
        {
            ProfileCommand profCommand = vkontakte.CommandsGenerator.GetUsers(APIManager.AccessData.UserId);
            APIManager.Profile = profCommand.ExecuteForList().FirstOrDefault();
            var paths = (new List<string>() { APIManager.Profile.photo, APIManager.Profile.photoMedium, APIManager.Profile.photoBig });
            var path = string.Empty;
            foreach (var p in paths)
                if (p.Count() > 0)
                {
                    path = p;
                    break;
                }
            if (path != string.Empty)
                Avatar = path;

            this.UserFullName = APIManager.Profile.last_name;
        }
        #endregion

        #region Share

        public void ShareInfo()
        {
            AudioCommand profCommand = vkontakte.CommandsGenerator.SendAudioToUserWall(APIManager.AccessData.UserId, 230);
            profCommand.ExecuteNonQuery();
        }

        #endregion

        private void OnUploadClick()
        {
            AudioUploadComman comm = CommandsGenerator.GetUploadServer();
            comm.Execute();
            var info = comm.UploadAudio(@"D:\Musik\", @"myzuka.ru_08_nobody_but_me.mp3");


            /*var audio = "%7B%22audio%22%3A%222e26475e89%22%2C%22time%22%3A138%2C%22artist%22%3A%22The+Human+Beinz%22%2C%22title%22%3A%22Nobody+But+Me%22%2C%22genre%22%3A24%2C%22album%22%3A%22The+Departed%22%2C%22bitrate%22%3A320%2C%22md5%22%3A%2292c8c6fdcd25c6998b3b86a1deaa99fa%22%2C%22kad%22%3A%2212005fec2aabe7208a349f46b98e96ff%5Cn%22%7D";
            audio = DecodeUrlString(audio);

            var info = new AudioUploadedInfo("536214", audio, "a71d783bad416ff57f703438eeaacf37");*/

            AudioUploadComman audioCommand = CommandsGenerator.SaveAudio(info);
            var fullstr = audioCommand.QueryString;
            var paramsAndtoken = @"" + audioCommand.GetParamsWithToken();
            //paramsAndtoken = @"?uid=15852307&fields=uid, first_name, last_name, nickname, sex, bdate, city, countryphoto, photo_medium, photo_big&access_token=f734a0848b5e7843e423e3acf72fd35736dfcff1e87b0e8aa5d22ddafd778c95c784dd8995b454d6e6b48";
            //paramsAndtoken = @"?server=536518&audio=%7B%22audio%22%3A%22a0e144777d%22%2C%22time%22%3A138%2C%22artist%22%3A%22The+Human+Beinz%22%2C%22title%22%3A%22Nobody+But+Me%22%2C%22genre%22%3A24%2C%22album%22%3A%22The+Departed%22%2C%22bitrate%22%3A320%2C%22md5%22%3A%2292c8c6fdcd25c6998b3b86a1deaa99fa%22%2C%22kad%22%3A%2212005fec2aabe7208a349f46b98e96ff%5Cn%22%7D&hash=21f7deda067d8930bf963f0159308348&artist=artist&title=title&access_token=69fedfbf0e43a0e942c5e1bf7158f25204224349a0e005890a9db1b63c181c9a69bc40ef3043b0ca1585a";
            //paramsAndtoken = System.IO.File.ReadAllText("file.txt");
            string asnwer = Reqeust.POST("https://api.vk.com/method/audio.save", paramsAndtoken);
            //audioCommand.Execute();
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
                backgroundWorker.RunWorkerAsync(sounds);
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
