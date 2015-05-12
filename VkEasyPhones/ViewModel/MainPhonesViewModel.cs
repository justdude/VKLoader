using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using vkontakte;

namespace VkEasyPhones.ViewModel
{
	public class MainPhonesViewModel : ViewModelBase
	{

		#region Properties
		public ObservableCollection<string> Cities { get; set;}
		public ObservableCollection<string> Countries { get; set;}
		#endregion

		#region Commands

		public ICommand SearchClick 
		{ 
			get
			{
				return new RelayCommand(OnSearchClick);
			}
		}

		public ICommand AutorizeClick
		{
			get
			{
				return new RelayCommand(OnAutorizeClick);
			}
		}

		private void OnAutorizeClick()
		{
			APIManager.Instance.Connect();
			APIManager.Instance.InitUser();
		}

		#endregion

		#region Methods


		protected void LoadCountries()
		{
			var profile = CommandsGenerator.ProfileCommands.GetUser(APIManager.Instance.Profile.uid);

			Console.WriteLine(profile.ToString());
		}

		private void OnSearchClick()
		{
		}

		#endregion
	}
}
