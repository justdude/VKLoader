using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace VKMusicSync.Handlers.CachedData
{
	public class InstancesData
	{
		private static InstancesData mvInstance;
		private BitmapImage mvSoundAuthor;

		public static InstancesData Instance
		{
			get
			{
				return mvInstance;
			}
		}

		static InstancesData()
		{
			mvInstance = new InstancesData();
		}

		public BitmapImage SoundAuthor
		{
			get
			{
				if (mvSoundAuthor == null)
				{
					mvSoundAuthor = GetDefaultMusicBitmap();
				}

				return mvSoundAuthor;
			}
		}
		public BitmapImage GetDefaultMusicBitmap()
		{
			try
			{
				var uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + Constants.Const.UnjnownAuthorPath);

				return new BitmapImage(uri);
			}
			catch(Exception)
			{
				try
				{
					return new BitmapImage(new Uri(VkDay.Constants.ImageDefaultURI));
				}
				catch(Exception)
				{
				}
			}

			return null;
		}
	}
}
