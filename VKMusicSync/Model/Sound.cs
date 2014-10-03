using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKMusicSync.Model
{

    public class Sound : IDownnloadedData, IEqualityComparer<Sound>
    {
        public IStateChanged InstanceWithEvents 
        { 
            get
            {
                return null;
            }
        }

        #region VK

        public string id { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
        public int duration { get; set; }
        public string url { get; set; }
        public string lyrics_id { get; set; }
        public string genre_id { get; set; }

        #endregion

        #region LastFm

        public string authorPhotoPath { get; set; }
        public List<string> similarArtists { get; set; }
        #endregion

        #region IO

        public double Size { get; set; }
        public double LoadedSize { get; set; }
        public bool IsLoadedToDisk { get; set; }
        #endregion


        #region CachedImage

        #endregion

        public Handlers.Synchronize.SyncStates State
        {
            get;
            set;
        }


        public string PathWithFileName
        {
            get
            {
                return Path + @"\" + FileName + FileExtention;
            }
        }

        private string mvFileName;
        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(mvFileName) || string.IsNullOrWhiteSpace(mvFileName))
                {
                    return this.artist + " - " + this.title;
                }
                return mvFileName;
            }
            set
            {
                mvFileName = value;
            }
        }

        public string FileExtention
        {
            get
            {
                return ".mp3";
            }
        }

        private string mvPath;
        public string Path
        {
            get
            {
                if (string.IsNullOrEmpty(mvPath) || string.IsNullOrWhiteSpace(mvPath))
                {
                    return url;
                }
                return mvPath;
            }
            set
            {
                mvPath = value;
            }
        }

        public string MD5
        {
            get;
            set;
        }

        public Sound()
        {
            this.artist = "no name";
            this.title = "track";
        }

        public Sound(string artist, string title)
        {
            this.artist = artist;
            this.title = title;
        }

        public Sound(string url)
        {
            this.artist = "no name";
            this.title = "track";
            this.url = url;
        }

        public override string ToString()
        {
            return String.Format("Sound:{0} : {1}", artist, title);
        }

        public override bool Equals(object obj)
        {
            Sound sound = obj as Sound;
            if (sound != null)
            {
                return sound.IsEqual(this);
            }
            return false;
           /* bool isNamesEquals = x.FileName.ToLower() == y.FileName.ToLower();
            return isNamesEquals;*/
        }

        public int GetHashCode(Sound obj)
        {
            throw new NotImplementedException();
        }

        public bool IsEqual(IDownnloadedData data)
        {
            var sound = data as Sound;
            float coeff = 0;

            bool isNamesEquals = FileName.ToLower().Trim() == data.FileName.ToLower().Trim();
            if (isNamesEquals)
                coeff = 5;

            if (sound != null)
            {
                bool isEqualArtist = artist.ToLower().Trim() == sound.artist.ToLower().Trim();
                bool isEqualSongName = title.ToLower().Trim() == sound.title.ToLower().Trim();

                bool isEqualFileDuration = duration == sound.duration && duration > 0;
                bool isEqualFileSize = Size == sound.Size && Size>0;

                if (isEqualArtist)
                    coeff += 1;
                if (isEqualSongName)
                    coeff += 3;

                if (isEqualFileDuration)
                    coeff += 0.5f;
                if (isEqualFileSize)
                    coeff += 0.5f;

            }

            return coeff >= 4f;
        }

        public bool Equals(Sound x, Sound y)
        {
            throw new NotImplementedException();
        }
    }
}
