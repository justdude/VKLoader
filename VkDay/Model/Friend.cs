using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkDay.Model
{
	public class Friend
	{
		public int id { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string nickname { get; set; }
		public string sex { get; set; }
		public string birthdate { get; set; }
		public string city { get; set; }
		public string country { get; set; }
		public string timezone { get; set; }
		public string photo { get; set; }
		public string photo_medium { get; set; }
		public string photo_big { get; set; }
	}
}
