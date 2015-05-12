using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkDay.Model
{
    public class AudioUploadedInfo
    {
        public string Server;
        public string Audio;
        public string Hash;
        public string Artist = null;
        public string Title = null;

        public AudioUploadedInfo(string server, string audio, string hash)
        {

            // присваиваем значения
            this.Server = server;
            this.Audio = audio;
            this.Hash = hash;
        }

    }
}
