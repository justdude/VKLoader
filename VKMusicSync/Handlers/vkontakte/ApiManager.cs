using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VKMusicSync;
using VKMusicSync.Model;

namespace vkontakte
{
	public class APIManager
	{
		public static VKApi vk { get; set;}

		public static AccessData AccessData { get; set; }
		public static Profile Profile { get; set; }

		public static void Connect()
		{
			var authWindow = new Auth();
			authWindow.ShowDialog();
		}

	}
}
