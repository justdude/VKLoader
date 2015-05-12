using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkDay.Model
{

		public class SoundBase
		{
			#region VK
			public string id { get; set; }
			public string artist { get; set; }
			public string title { get; set; }
			public int duration { get; set; }
			public string url { get; set; }
			public string lyrics_id { get; set; }
			public string genre_id { get; set; }
			#endregion
		}
    
}
