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

namespace VKMusicSync.ModelView
{
    class SoundDownloaderMovelView: ViewModelBase
    {

        private Profile Profile = new Profile();

        #region Binding variables


        //public System.Windows.Visibility ProgressVisibility = System.Windows.Visibility.Visible;


        public ObservableCollection<SoundModelView> Sounds { get; set; }


        public string UserFullName
        {
            get
            { return Profile.first_name + " " + Profile.last_name; }
            set
            {
                OnPropertyChanged("UserFullName");
            }
        }

        private System.Windows.Media.Imaging.BitmapFrame avatar;

        public System.Windows.Media.Imaging.BitmapFrame Avatar
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
                OnPropertyChanged("ProgressPercentage");
            }
        }
        #endregion

        #region Click Commands

        private DelegateCommand auth;
        public ICommand AuthClick
        {
            get
            {
                if (auth == null)
                {
                    auth = new DelegateCommand(OnAuthClick);
                }
                return auth;
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

        private DelegateCommand sync;
        public ICommand SyncClick
        {
            get
            {
                if (sync == null)
                {
                    sync = new DelegateCommand(OnSyncClick);
                }
                return sync;
            }

        }
        #endregion

        #region Constructor
        public SoundDownloaderMovelView(List<Sound> sounds)
        {
            SetSounds(sounds);
        }

        private void SetSounds(List<Sound> sounds)
        {
            Sounds = new ObservableCollection<SoundModelView>(sounds.Select(s => new SoundModelView(s)));
        }
        #endregion



        private void OnSyncClick()
        {
            Window_Loaded();
        }

        private void OnSettingsClick()
        {
            System.Windows.Forms.MessageBox.Show("SettingsClick");
        }

        private void OnAuthClick()
        {
            var authWindow = new Auth();
            authWindow.ShowDialog();
            //System.Windows.Forms.MessageBox.Show("OnSyncClick");
        }
        
        List<Sound> sounds = new List<Sound>();

        private void Window_Loaded()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += this.Init;
            backgroundWorker.RunWorkerCompleted += this.InitDone;
            backgroundWorker.RunWorkerAsync();
        }

        

        private void Init(object sender, DoWorkEventArgs e)
        {

            int count = int.Parse(APIManager.vk.GetAudioCountFromUser(APIManager.vk.UserId, 
                                                                      false).SelectSingleNode("response")
                                                                            .InnerText);
            ProfileCommand profCommand = vkontakte.CommandsGenerator.GetUsers(APIManager.AccessData.UserId);
            this.Profile = profCommand.Execute().FirstOrDefault();
            var paths = (new List<string>() { Profile.photo, Profile.photoMedium, Profile.photoBig });
            var path = string.Empty;
            foreach(var p in paths)
                if (p.Count()>0) 
                {
                    path = p;
                    break;
                }
            /*if (path!=string.Empty)
                this.Avatar = new ImageModel(path).Image;*/
            this.UserFullName = Profile.last_name;

            if (count > 0)
            {
                AudioCommand command = vkontakte.CommandsGenerator.GetAudioFromUser(APIManager.vk.UserId, false, 0, count);
                command.OnCommandExecuting += OnCommandLoading;
                sounds = command.Execute();
            }
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
        }


    }
}
