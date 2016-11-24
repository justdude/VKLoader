using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLastFm;
using VKMusicSync.Model;
using VKMusicSync.ViewModel;
namespace VKMusicSync.Handlers
{
	public class ItemHelper
	{
		public static void FillSound(LastFmApi lastFmApi, Sound item)
		{
			try
			{
				var artist = lastFmApi.Artist.GetInfo(item.artist);
				item.authorPhotoPath = artist.Images[2].Value; // little spike 
				//item.PhotoPath = artist.Images[3].Value;// little spike 

			}
			catch (DotLastFm.Api.Rest.LastFmApiException)
			{
			}
		}

		public static void FillDataInfo(List<VKMusicSync.Model.Sound> items)
		{
			double coef = 1024 * 1024;
			foreach (var item in items)
			{
				item.Size = ComputeSize(item.Path) / coef;
			}
		}


		public static Int64 ComputeSize(string link)
		{
			System.Net.WebClient client = new System.Net.WebClient();
			client.OpenRead(link);
			return Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
		}
	}
}
