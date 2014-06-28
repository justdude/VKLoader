using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVM;
using VKMusicSync.Model;
using System.Windows.Input;
using System.Net;

namespace VKMusicSync.ModelView
{
    class SettingsModelView : ViewModelBase
    {

        public string UserFullName
        {
            get
            {
                return vkontakte.APIManager.Profile.ToString();
            }
            set
            {
                OnPropertyChanged("UserFullName");
            }
        }



        public int ThreadMax
        {
            get
            {
                return System.Environment.ProcessorCount * 4;
            }
        }

        public int ThreadCountToUse
        {
            get
            {
                return Properties.Settings.Default.ThreadCountToUse;
            }
            set
            {
                Properties.Settings.Default.ThreadCountToUse = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("ThreadCountToUse");
            }
        }



        public string DownloadFolderPath
        {
            get
            {
                return Properties.Settings.Default.DownloadFolderPath;
            }
            set
            {
                Properties.Settings.Default.DownloadFolderPath = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("DownloadFolderPath");
            }
        }

        public string BackgroundPath
        {
            get
            {
                return Properties.Settings.Default.BackgroundPath;
            }
            set
            {
                Properties.Settings.Default.BackgroundPath = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("BackgroundPath");
            }
        }

        private WebProxy proxy;
        public WebProxy Proxy
        {
            get
            {
                if (proxy == null)
                {
                    proxy = new WebProxy(Properties.Settings.Default.ProxyAdress, 
                                         int.Parse(Properties.Settings.Default.ProxyPort));
                    proxy.Credentials = Credential;
                    OnPropertyChanged("Proxy");
                }
                return proxy;
            }
            set
            {
                proxy = value;
                OnPropertyChanged("Proxy");
            }
        }
        private NetworkCredential credential;
        public NetworkCredential Credential
        {
            get
            {
                if (credential == null)
                {
                    credential = new NetworkCredential(Properties.Settings.Default.ProxyName, 
                                                       Properties.Settings.Default.ProxyPassword);
                    OnPropertyChanged("Credential");
                }
                return credential;
            }
            set
            {
                credential = value;
                OnPropertyChanged("Credential");
            }
        }


        private DelegateCommand selectDownloadFolderClick;
        public ICommand SelectDownloadFolderClick
        {
            get
            {
                if (selectDownloadFolderClick == null)
                {
                    selectDownloadFolderClick = new DelegateCommand(OnSelectDownloadFolderClick);
                }
                return selectDownloadFolderClick;
            }
        }

        private DelegateCommand selectBackgroundPathClick;
        public ICommand SelectBackgroundPathClick
        {
            get
            {
                if (selectBackgroundPathClick==null)
                {
                    selectBackgroundPathClick = new DelegateCommand(OnSelectBackgroundPathClick);
                }
                return selectBackgroundPathClick;
            }
        }


        private DelegateCommand applySelectedBackground;
        public ICommand ApplySelectedBackground
        {
            get
            {
                if (applySelectedBackground == null)
                {
                    applySelectedBackground = new DelegateCommand(OnApplySelectedBackground);
                }
                return applySelectedBackground;
            }
        }

        private DelegateCommand loginClick;
        public ICommand LoginClick
        {
            get
            {
                if (loginClick == null)
                {
                    loginClick = new DelegateCommand(OnLoginClick, CanLogin);
                }
                return loginClick;
            }
        }

        private DelegateCommand exitClick;
        public ICommand ExitClick
        {
            get
            {
                if (exitClick == null)
                {
                    exitClick = new DelegateCommand(OnExitClick,CanExit);
                }
                return exitClick;
            }
        }

        private bool CanLogin()
        {
            return vkontakte.APIManager.AccessData == null;
        }

        private bool CanExit()
        {
            return vkontakte.APIManager.AccessData != null;
        }

        private void OnExitClick()
        {
            vkontakte.APIManager.AccessData = null;
            vkontakte.APIManager.Profile = null;
            
        }

        private void OnLoginClick()
        {
            Auth form = new Auth();
            form.ShowDialog();
        }



        private void OnApplySelectedBackground()
        {

        }

        private void OnSelectBackgroundPathClick()
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";
            dialog.ShowDialog();
            if (dialog.CheckFileExists)
            {
                this.BackgroundPath = dialog.FileName;
            }
        }
        private void OnSelectDownloadFolderClick()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowDialog();
            if (System.IO.Directory.Exists(dialog.SelectedPath)==true)
                this.DownloadFolderPath = dialog.SelectedPath+@"\Audio\";
        }


    }
}
