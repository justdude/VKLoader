using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLastFm;
using DotLastFm.Api;
namespace VKMusicSync.Handlers
{
    class LastFmHandler
    {

        private static LastFmApi api;
        public static LastFmApi Api
        { 
            get
            {
                if (api == null) api = new LastFmApi(new TestLastFmConfig());
                return api;
            }
            private set
            {
                api = value;
            }
        }


    }
}
