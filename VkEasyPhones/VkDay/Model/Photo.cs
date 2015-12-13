using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKLib.Model
{
	public class Photo
	{
		public string id { get; set; }
		public string album_id { get; set; }
		public string owner_id { get; set; }
		public string photo_75 { get; set; }
		public string photo_130 { get; set; }
		public string photo_1280 { get; set; }
		public string photo_2560 { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public string text { get; set; }
		public float date { get; set; }
		public int user_likes { get; set; }

		public string path;

	}
}
