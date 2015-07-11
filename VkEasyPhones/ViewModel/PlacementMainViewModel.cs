using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using VkDay;
using VkEasyPhones.VkDay.Model;

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
				InformListeners();

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
				InformListeners();

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
				InformListeners();

				RaisePropertyChanged(()=>CityName);
			}
		}

		private void InformListeners()
		{

			if (SelectedCity == null || SelectedCountry == null)
				return;

			if (SelectedCountry.Location == null || SelectedCity.Location == null)
				return;

			var message = new VkEasyPhones.Messages.PlacementMessage()
			{
				CityID = SelectedCity.Location.cid,
				CountryID = SelectedCountry.Location.cid,
			};

			MessengerInstance.Send<VkEasyPhones.Messages.PlacementMessage>(message);
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
			
			if(countryId == "2")
			{
				Cities.Add(new PlaceViewModel() 
				{ 
					Location = new City() 
					{ 
						cid = "2201", 
						title = "Кировоград" 
					}
				});
			}
			SelectedCity = Cities.FirstOrDefault();
		}

		public string mvSelectedCityId { get; set; }
	}
}
