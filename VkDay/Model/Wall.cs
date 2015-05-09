using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkDay.Model
{
	public class Wall
	{
		public int id { get; set; }
		public int from_id { get; set; }
		public int to_id { get; set; }
		public int date { get; set; }
		public string text { get; set; }
		public Object comments { get; set; }
		public Object likes { get; set; }
		public Object reposts { get; set; }
	}
}
