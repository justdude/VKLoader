using DotLastFm;
using DotLastFm.Api;

namespace VKMusicSync.Handlers.LastFm
{
	class LastFmHandler
	{
		private static LastFmApi api;
		public static LastFmApi Api
		{
			get { return api ?? (api = new LastFmApi(new TestLastFmConfig())); }
		}


	}
}
