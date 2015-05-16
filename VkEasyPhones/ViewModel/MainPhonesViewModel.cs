using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using VkDay;
using System.IO;
using VkDay.Model;
using System.Threading;
using VkEasyPhones.VkDay.Model;
using VkEasyPhones.Converters;
using VkEasyPhones.Enumartions;

namespace VkEasyPhones.ViewModel
{
	public class MainPhonesViewModel : ViewModelBase
	{
		#region Fields
		private enPeoplesTypes mvPeoplesSearchingType;
		private int mvMaxAge;
		private int mvMinAge;
		#endregion

		#region Properties
		public bool IsLogined
		{
			get
			{
				return APIManager.Instance.IsCanLogin == false;
			}
		}

		public bool IsLoaded
		{
			get
			{
				return APIManager.Instance.IsCanLogin == false;
			}
		}

		public int MaxAge
		{
			get
			{
				return mvMaxAge;
			}
			set
			{
				if (mvMaxAge == value)
					return;

				mvMaxAge = value;

				RaisePropertyChanged(() => MaxAge);
			}
		}

		public int MinAge
		{
			get
			{
				return mvMinAge;
			}
			set
			{
				if (mvMinAge == value)
					return;

				mvMinAge = value;

				RaisePropertyChanged(() => MinAge);
			}
		}

		public enPeoplesTypes PeoplesSearchingType
		{
			get
			{
				return mvPeoplesSearchingType;
			}
			set
			{
				if (mvPeoplesSearchingType == value)
					return;

				mvPeoplesSearchingType = value;

				RaisePropertyChanged(() => PeoplesSearchingType);
			}
		}

		#endregion

		#region Commands

		public ICommand SearchClick
		{
			get
			{
				return new RelayCommand(OnSearchClick, IsCanUseApi);
			}
		}

		private bool IsCanUseApi()
		{
			return IsLogined;
		}

		public ICommand AutorizeClick
		{
			get
			{
				return new RelayCommand(OnAutorizeClick, IsCanLogin);
			}
		}

		private bool IsCanLogin()
		{
			return IsLogined == false;
		}

		#endregion


		#region Ctr
		public MainPhonesViewModel()
		{
			PeoplesSearchingType = enPeoplesTypes.All;
			MinAge = 16;
			MaxAge = 80;
		}
		#endregion


		#region Methods

		private void OnAutorizeClick()
		{
			APIManager.Instance.OnUserLoaded += Instance_OnUserLoaded;
			APIManager.Instance.Connect();

			if (APIManager.Instance.AccessData == null)
				return;

			APIManager.Instance.InitUser();
		}

		private void Instance_OnUserLoaded(VKApi.ConnectionState obj)
		{
			if (obj == VKApi.ConnectionState.Loaded)
			{
				RaisePropertyChanged(() => IsLoaded);
			}
		}

		private void OnSearchClick()
		{
			ThreadPool.QueueUserWorkItem((d) =>
			{
				List<Profile> users = new List<Profile>();

				for (int i = 0; i < 3; i++)
				{
					var tempUsers = CommandsGenerator.ProfileCommands.Search("", 1000, i);
					users.AddRange(tempUsers);
				}

				var withPhones = users.Where(p => p.IsHasNonEmptyNumbers && p.city == "2201");

				StringBuilder bd = new StringBuilder();
				foreach (var item in withPhones)
				{
					bd.AppendLine(item.FullName + "(" + item.uid + ")" + " : " + item.mobile_phone + " ; " + item.home_phone);
				}

				File.WriteAllText("file.txt", bd.ToString());
				System.Diagnostics.Process.Start("file.txt");
			});
		}

		#endregion

		
	}
}
