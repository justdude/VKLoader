﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVM;
using VKMusicSync.Model;
using DotLastFm;
using System.Windows.Media.Imaging;
using VKMusicSync.Handlers.Synchronize;
namespace VKMusicSync.ModelView
{
    public class SoundModelView : ViewModelBase, IDownnloadedData
    {
        public Sound Sound { get; set; }
        /*
         public string UserId { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
        public string duration { get; set; }
        public string url { get; set; }
        public string lyrics_id { get; set; }
        public string genre_id { get; set; }
         */

        #region Sync States 
        private SyncStates state = SyncStates.IsNeedDownload;
        public SyncStates State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }
        #endregion

        public static bool FreezeClick = false;

        private bool ischecked = true;

        public bool Checked
        {
            get
            {
                return ischecked;
            }
            set
            {
                //if (FreezeClick == false)
                if (ischecked!=value)
                {
                    ischecked = value;
                    OnPropertyChanged("Checked");
                }
            }
        }

        private bool currentProgressVisibility = false;
        public bool CurrentProgressVisibility
        {
            get
            {
                return currentProgressVisibility;
            }
            set
            {
                if (currentProgressVisibility!=value)
                {
                    currentProgressVisibility = value;
                    OnPropertyChanged("CurrentProgressVisibility");
                }
            }

        }

        public string Artist
        {
            get { return Sound.artist; }
            set
            {
                Sound.artist = value;
                OnPropertyChanged("Artist");
            }
        }

        public string Title
        {
            get { return Sound.title; }
            set
            {
                Sound.title = value;
                OnPropertyChanged("Title");
            }
        }

        public int Duration
        {
            get { 
                return Sound.duration; 
            }
            set
            {
                Sound.duration = value;
                OnPropertyChanged("Duration");
            }
        }

        public int Quality
        {
            get { return 192; }
            set
            {
                //Sound.Quality = value;
                OnPropertyChanged("Duration");
            }
        }

        public double Size
        {
            get { return Sound.Size; }
            set
            {
                Sound.Size = value;
                OnPropertyChanged("Size");
            }
        }

        public double LoadedSize
        {
            get { return Sound.LoadedSize; }
            set
            {
                Sound.LoadedSize = value;
                OnPropertyChanged("LoadedSize");
            }
        }



        public string PhotoPath
        {
            get 
            {
                return Sound.authorPhotoPath; 
            }
            set
            {
                if (value!=Sound.authorPhotoPath)
                {
                    Sound.authorPhotoPath = value;
                    OnPropertyChanged("PhotoPath");
                }
            }
        }


        private BitmapImage mvImage = null;

        public BitmapImage Photo
        {
            get
            {
                if (mvImage == null)
                {
                    if (Sound.authorPhotoPath.Length > 0)
                        mvImage = new BitmapImage(new Uri(Sound.authorPhotoPath));
                }
                return mvImage;
            }
            set
            {
                if (value != mvImage)
                {
                    mvImage= value;
                    OnPropertyChanged("Photo");
                }
            }
        }

        public bool SyncState
        {
            get { return Sound.SyncState; }
            set
            {
                if (Sound.SyncState != value)
                {
                    Sound.SyncState = value;
                    OnPropertyChanged("SyncState");

                    if (Sound.SyncState == false)
                        CurrentProgressVisibility = false;
                    else
                        CurrentProgressVisibility = true;
                    VKMusicSync.ModelView.SoundDownloaderMovelView.Instance.UpdateList();
                }
            }
        }
        //SimilarArtist


        public List<string> SimilarArtist
        {
            get { return Sound.similarArtists; }
            set
            {
                if (Sound.similarArtists != value)
                {
                    Sound.similarArtists = value;
                    OnPropertyChanged("SimilarArtist");
                }
            }
        }

        public SoundModelView(Sound sound)
        {
            this.Sound = sound;
        }

        public string GenerateFileName()
        {
            return this.Sound.GenerateFileName();
        }

        public string GenerateFileExtention()
        {
            return this.Sound.GenerateFileExtention();
        }

        public string GetUrl()
        {
            return this.Sound.GetUrl();
        }
    }
}
