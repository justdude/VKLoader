using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVM;
using VKMusicSync.Model;

namespace VKMusicSync.ModelView
{
    class SoundModelView : ViewModelBase
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
            get { return Sound.duration; }
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
            get { return 2.12; }
            set
            {
                //Sound.Quality = value;
                OnPropertyChanged("Size");
            }
        }

        public double LoadedSize
        {
            get { return 1.59; }
            set
            {
                //Sound.Quality = value;
                OnPropertyChanged("LoadedSize");
            }
        }

        public bool SyncState
        {
            get { return true; }
            set
            {
                //Sound.Quality = value;
                OnPropertyChanged("LoadedSize");
            }
        }

        public SoundModelView(Sound sound)
        {
            this.Sound = sound;
        }



    }
}
