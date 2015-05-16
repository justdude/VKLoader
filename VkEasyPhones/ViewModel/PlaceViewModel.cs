using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VkEasyPhones.VkDay.Model;

namespace VkEasyPhones.ViewModel
{
	public class PlaceViewModel : ViewModelBase
	{
		#region Properties

		public ILocation Location { get; set;}

		public string PlaceName
		{
			get
			{
				if (Location == null)
					return string.Empty;

				return Location.title;
			}
		}

		#endregion

		#region Ctr
		public PlaceViewModel()
		{

		}

		public PlaceViewModel(ILocation location):this()
		{
			Location = location;
			RaisePropertyChanged("PlaceName");
		}
		#endregion

	}
}
