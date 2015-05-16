using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using VkDay;

namespace VkEasyPhones.ViewModel
{
	public class PlacementMainViewModel : ViewModelBase
	{
		private PlaceViewModel mvSelectedCity;
		private string mvCityName;
		private PlaceViewModel mvSelectedCountry;
		private bool mvIsLoaded;

		public ObservableCollection<PlaceViewModel> Cities { get; set; }
		public ObservableCollection<PlaceViewModel> Countries { get; set; }

		public PlaceViewModel SelectedCountry
		{
			get
			{
				return mvSelectedCountry;
			}
			set
			{
				if (value == mvSelectedCountry)
					return;

				mvSelectedCountry = value;
				UpdateCities(SelectedCountry.Location.cid);

				RaisePropertyChanged(()=>SelectedCountry);
			}
		}

		public PlaceViewModel SelectedCity 
		{ 
			get
			{
				return mvSelectedCity;
			}
			set
			{
				if (value == mvSelectedCity)
					return;

				mvSelectedCity = value;

				RaisePropertyChanged(() => SelectedCity);
			} 
		}

		public string CityName
		{
			get
			{
				return mvCityName;
			}
			set
			{
				if (mvCityName == value)
					return;

				mvCityName = value;

				RaisePropertyChanged(()=>CityName);
			}
		}

		public bool IsLoaded
		{
			get
			{
				return mvIsLoaded;
			}
			set
			{
				if (mvIsLoaded == value)
					return;

				mvIsLoaded = value;

				RaisePropertyChanged(() => IsLoaded);
			}
		}

		//public string SelectedCityId
		//{
		//	get
		//	{
		//		if (SelectedCity == sect)
		//		return SelectedCity.Location.cid;
		//	}
		//}//SelectedCityId

		#region Ctr

		public PlacementMainViewModel()
		{
			APIManager.Instance.OnUserLoaded += Instance_OnUserLoaded;
			Countries = new ObservableCollection<PlaceViewModel>();
			Cities = new ObservableCollection<PlaceViewModel>();
		}

		void Instance_OnUserLoaded(VKApi.ConnectionState obj)
		{
			if (obj != VKApi.ConnectionState.Loaded)
				return;

			LoadCountries();

			IsLoaded = true;
		}

		#endregion

		protected void LoadCountries()
		{
			var countries = CommandsGenerator.GeolocationCommands.GetCountries(false, GeolocationCommands.CountriesCodes);

			Countries.Clear();
			countries.ForEach(p => Countries.Add(new PlaceViewModel(p)));

			SelectedCountry = Countries[1];

			if (SelectedCountry == null || SelectedCountry.Location == null)
				return;

			string countryId = SelectedCountry.Location.cid;
			UpdateCities(countryId);
		}

		private void UpdateCities(string countryId)
		{
			var cities = CommandsGenerator.GeolocationCommands.GetSities(countryId, string.Empty);
			Cities.Clear();
			cities.ForEach(p => Cities.Add(new PlaceViewModel(p)));
			SelectedCity = Cities.FirstOrDefault();
		}

		public string mvSelectedCityId { get; set; }
	}
}
