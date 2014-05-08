using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKMusicSync.Model
{

    public class Sound : IData
    {
        public string id { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
        public int duration { get; set; }
        public string url { get; set; }
        public string lyrics_id { get; set; }
        public string genre_id { get; set; }


        public Sound()
        {

        }

        public Sound(string artist, string title)
        {
            this.artist = artist;
            this.title = title;
        }

        public Sound(string url)
        {
            this.url = url;
        }

        public override string ToString()
        {
            return String.Format("Sound:{0} : {1}", artist, title);
        }

        public string GetUrl()
        {
            return url;
        }

        public string GenerateFileName()
        {
            string value = this.artist + " - " + this.title;
            value = value.Replace("&amp;", "&");
            return value;
        }
        public string GenerateFileExtention()
        {
            return ".mp3";
        }
    }
}
