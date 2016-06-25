using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VKLib;
using VKLib.Model;

namespace VKLib
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
		public AccessDataInfo AccessDataInfo { get; private set; }
		public Profile Profile { get; private set; }
		public bool IsCanLogin
		{
			get
			{
				return AccessDataInfo == null;
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
					Profile = CommandsGenerator.ProfileCommands.GetUser(AccessDataInfo.UserId);
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
			AccessDataInfo = null;
			Profile = null;
		}

		private void OnAccessTokenLoaded(string url)
		{
			AccessDataInfo = new AccessDataInfo(url);
			if (API == null)
				API = new VKApi();
			API.Init(AccessDataInfo);
		}

		private void RaiseOnUserLoaded(VKApi.ConnectionState state)
		{
			if (OnUserLoaded == null)
				return;

			OnUserLoaded(state);
		}
	}
}
