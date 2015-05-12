using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VkDay;
using VkDay.Model;

namespace VkDay
{
	public class APIManager
	{
		private static APIManager mvInstance;
		public static APIManager Instance
		{
			get
			{
				if (mvInstance == null)
				{
					mvInstance = new APIManager();
				}
				return mvInstance;
			}
		}

		private object modSync = new object();

		public VKApi API { get; private set; }
		public AccessData AccessData { get; private set; }
		public Profile Profile { get; private set; }
		public bool IsCanLogin
		{
			get
			{
				return AccessData == null;
			}
		}

		public event Action<VKApi.ConnectionState> OnUserLoaded;

		private APIManager()
		{
			API = new VKApi();
		}

		public void Connect()
		{
			var authWindow = new Auth();
			authWindow.auth.OnAccessTokenLoaded += OnAccessTokenLoaded;
			authWindow.ShowDialog();
		}


		public void InitUser()
		{
			lock (modSync)
			{
				try
				{
					Profile = CommandsGenerator.ProfileCommands.GetUser(AccessData.UserId);
					RaiseOnUserLoaded(VKApi.ConnectionState.Loaded);
				}
				catch (Exception ex)
				{
					RaiseOnUserLoaded(VKApi.ConnectionState.Failed);
				}
				
			}
		}

		public void TryExit()
		{
			//http://api.API.com/oauth/logout
			AccessData = null;
			Profile = null;
		}

		private void OnAccessTokenLoaded(string url)
		{
			AccessData = new AccessData(url);
			if (API == null)
				API = new VKApi();
			API.Init(AccessData);
		}

		private void RaiseOnUserLoaded(VKApi.ConnectionState state)
		{
			if (OnUserLoaded == null)
				return;

			OnUserLoaded(state);
		}
	}
}
