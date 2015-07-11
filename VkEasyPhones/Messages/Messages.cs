using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkEasyPhones
{
	public class Messages
	{
		public class PlacementMessage : MessageBase
		{
			public string CityID { get; set; }
			public string CountryID { get; set; }
		}
	}
}
