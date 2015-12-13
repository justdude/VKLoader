using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkEasyPhones.VKLib.Model
{
	public class Country : ILocation
	{
		public string cid { get; set; }
		public string title { get; set; }

		public override string ToString()
		{
			return title;
		}
	}
}
