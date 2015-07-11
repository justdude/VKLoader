using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVM;
using VKMusicSync.Model;
using System.Windows.Input;
using System.Net;
using System.Collections.ObjectModel;

namespace VKMusicSync.ModelView
{
    class SettingsModelView : AdwancedViewModelBase
    {

        public string UserFullName
        {
            get
            {
                return VkDay.APIManager.Instance.Profile.ToString();
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

        #region Proxy
        private WebProxy proxy;
        public WebProxy Proxy
        {
            get
            {
                if (proxy == null)
                {
                    proxy = new WebProxy(Properties.Settings.Default.ProxyAdress, 
                                         int.Parse(Properties.Settings.Default.ProxyPort));
                    if (Properties.Settings.Default.UseProxyCredintial)
                    {
                        proxy.Credentials = Credential;
                    }
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

        public bool UseCredintial
        {
            get
            {
                return Properties.Settings.Default.UseProxyCredintial;
            }
            set
            {
                Properties.Settings.Default.UseProxyCredintial = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("UseCredintial");
            }
        }

        public bool UseProxy
        {
            get
            {
                return Properties.Settings.Default.UseProxy;
            }
            set
            {
                Properties.Settings.Default.UseProxy = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("UseProxy");
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


        public string Adress
        {
            get
            {
                return Properties.Settings.Default.ProxyAdress;
            }
            set
            {
                Properties.Settings.Default.ProxyAdress = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("Adress");
            }
        }

        public string Port
        {
            get
            {
                return Properties.Settings.Default.ProxyPort;
            }
            set
            {
                Properties.Settings.Default.ProxyPort = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("Port");
            }
        }

        public string Login
        {
            get
            {
                return Properties.Settings.Default.ProxyName;
            }
            set
            {
                Properties.Settings.Default.ProxyName = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("Login");
            }
        }


        public string Password
        {
            get
            {
                return Properties.Settings.Default.ProxyPassword;
            }
            set
            {
                Properties.Settings.Default.ProxyPassword = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("Password");
            }
        }
        #endregion


        public List<System.Windows.Media.SolidColorBrush> ColorsScheme
        {
            get
            {
                return VKMusicSync.Handlers.ColorJam.AllCollors;
            }
        }

        #region Commands
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
        #endregion


        #region Checkers
        private bool CanLogin()
        {
			return VkDay.APIManager.Instance.IsCanLogin;
        }

        private bool CanExit()
        {
			return VkDay.APIManager.Instance.IsCanLogin == false;
        }
        #endregion

        private void OnExitClick()
        {
			VkDay.APIManager.Instance.TryExit();
        }

        private void OnLoginClick()
        {
			VkDay.APIManager.Instance.Connect();
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
