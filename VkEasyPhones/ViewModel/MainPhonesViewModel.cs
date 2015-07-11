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
using System.Windows.Threading;
using VkEasyPhones.Constants;
using VkEasyPhones.Helpers;
using System.Windows;

namespace VkEasyPhones.ViewModel
{
	public class MainPhonesViewModel : ViewModelBase
	{
		#region Fields
		private enPeoplesTypes mvPeoplesSearchingType;
		private int mvMaxAge;
		private int mvMinAge;
		private bool mvIsLoading;
		private enFoundStates mvFoundStates;
		private int modFoundResultCount = 0;
		private string mvSelectedCityId;
		private string mvTargetPath;
		private int mvCountOfPages;
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

		public bool IsLoading
		{
			get
			{
				return mvIsLoading;
			}
			set
			{
				if (mvIsLoading == value)
					return;

				mvIsLoading = value;
				RaisePropertyChanged(() => IsLoading);
			}
		}
		public string ResultText
		{
			get
			{
				return string.Format(Translates.ResultsFound, modFoundResultCount);
			}
		}

		public string TargetPath
		{
			get { return mvTargetPath; }
			set { mvTargetPath = value; RaisePropertyChanged(() => TargetPath); }
		}

		public string SelectedCityId
		{
			get
			{
				return mvSelectedCityId;
			}
			set
			{
				if (mvSelectedCityId == value)
					return;

				mvSelectedCityId = value;

				RaisePropertyChanged(() => SelectedCityId);
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
		//

		public int CountOfPages
		{
			get
			{
				return mvCountOfPages;
			}
			set
			{
				if (mvCountOfPages == value)
					return;

				mvCountOfPages = value;

				RaisePropertyChanged(() => CountOfPages);
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

		public enFoundStates FoundStates
		{
			get
			{
				return mvFoundStates;
			}
			set
			{
				if (mvFoundStates == value)
					return;

				mvFoundStates = value;

				RaisePropertyChanged(() => FoundStates);
			}
		}//FoundStates
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

			CountOfPages = 10;

			MessengerInstance.Register<VkEasyPhones.Messages.PlacementMessage>(this, OnLocationChange);
			TargetPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}
		#endregion


		#region Methods

		private void OnAutorizeClick()
		{
			IsLoading = true;
			APIManager.Instance.OnUserLoaded += Instance_OnUserLoaded;
			APIManager.Instance.Connect();

			if (APIManager.Instance.AccessData == null)
				return;

			APIManager.Instance.InitUser();
			IsLoading = false;
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
			List<Profile> users = new List<Profile>();

			if (string.IsNullOrEmpty(TargetPath) || System.IO.Directory.Exists(TargetPath) == false)
			{
				MessageBox.Show(" Неверный путь ");
				return;
			}
			if (string.IsNullOrEmpty(TargetPath))
			{
				MessageBox.Show(" Город неверен ");
				return;
			}
			if (CountOfPages <=1)
			{
				MessageBox.Show(" Нужно больше страниц ");
				return;
			}
			//CountOfPages

			int pagesCount = CountOfPages;
			int threads = 3;
			string cityId = SelectedCityId;//"2201";
			string filePath = TargetPath + @"\" + "FOUND_USERS.txt";
			string xlsPath = TargetPath + @"\" + "FOUND_USERS.xls";

			ChangeState(0, enFoundStates.Searching, true);

			ThreadPool.QueueUserWorkItem((d) =>
			{
				Fill(users, pagesCount, threads);

				var filtered = Filter(users, cityId);

				WriteToFile(filtered, filePath);


				try
				{
					CExcelHelper.WriteToExcelSpreadSheets(xlsPath, filtered);
					System.Diagnostics.Process.Start(xlsPath);
				}
				catch
				{
					MessageBox.Show("Не удалось обработать Excel файл. Воспользуйтесь txt файлом");
				}

				ChangeState(filtered.Count(), enFoundStates.ShowResults, false);

				
			});
		}

		private static IEnumerable<Profile> Filter(List<Profile> users, string cityId)
		{
			var withPhones = users.Where(p => p.IsHasNonEmptyNumbers && p.city == cityId).GroupBy(p=>p.uid).Select(gpr=>gpr.FirstOrDefault());
			return withPhones;
		}

		private void ChangeState(int countFound, enFoundStates state, bool isLoading)
		{
			Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
			{
				//IsLoaded = true;
				modFoundResultCount = countFound;
				IsLoading = isLoading;
				FoundStates = state;
			}));
		}

		private static void WriteToFile(IEnumerable<Profile> users, string path)
		{
			StringBuilder bd = new StringBuilder();
			foreach (var item in users)
			{
				bd.AppendLine(item.FullName + "(" + item.uid + ")" + " : " + item.mobile_phone + " ; " + item.home_phone);
			}

			File.WriteAllText(path, bd.ToString());
		}

		private static void Fill(List<Profile> users, int pagesCount, int threads)
		{
			for (int i = 0; i < pagesCount; i++)
			{
				var tempUsers = CommandsGenerator.ProfileCommands.Search("", 1000, i);
				users.AddRange(tempUsers);
			}
		}

		#endregion



		public void OnLocationChange(VkEasyPhones.Messages.PlacementMessage mess)
		{
			SelectedCityId = mess.CityID;
		}
	}
}
