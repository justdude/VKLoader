using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkDay.vkontakte;
using VKLib;
using VKLib.Model;

namespace VKLib.vkontakte
{
	public class VkWrapper : IVkWrapper
	{
		#region Fields

		private Profile mvUserData;

		#endregion

		#region Properties
		public VKApi VkInstance { get; private set; }

		public Profile UserProfile { get; private set; }

		//VKLib.APIManager.VkInstance.UserProfile.FullName
		public string FullName { get; set; }

		public AccessDataInfo AccessInfo { get; private set; }

		#endregion Properties

		#region Ctr.

		public VkWrapper()
		{
			VkInstance = new VKApi();
		}

		public VkWrapper(VKApi vkInstance)
		{
			VkInstance = vkInstance;
		}

		#endregion

		#region Events

		public event Action<object, VKApi.ConnectionState> AutorizedAction;

		public event Action<object, VKApi.ConnectionState> UserLoadedAction;

		//vkWrapper.VKLib.APIManager.VkInstance.API.OnConnectionStateChanged += API_OnConnectionStateChanged;
		//public event Action<VKLib.VKApi.ConnectionState> ConnectionStateChanged;

		#endregion

		#region Methods

		public void ShowAutorizationWindow()
		{
			Clear();

			var authWindow = new Auth();

			authWindow.auth.OnAccessTokenLoaded += AccessTokenLoaded;

			authWindow.ShowDialog();
		}

		private void AccessTokenLoaded(string url)
		{
			AccessInfo = new AccessDataInfo(url);

			VkInstance.Init(AccessInfo);

			OnAutorizedAction(VKApi.ConnectionState.Loaded);
		}

		public void InitUser()
		{
			UserProfile = CommandsGenerator.ProfileCommands.GetUser(AccessInfo.UserId);

			OnUserLoadedAction(this, VKApi.ConnectionState.Loaded);
		}

		public void Clear()
		{
			//http://api.API.com/oauth/logout
			AccessInfo = null;
			UserProfile = null;
		}

		#endregion

		protected virtual void OnAutorizedAction(VKApi.ConnectionState obj)
		{
			if (AutorizedAction == null)
				return;

			AutorizedAction(this, obj);
		}
		protected virtual void OnUserLoadedAction(object arg1, VKApi.ConnectionState arg2)
		{
			if (UserLoadedAction == null)
				return;

			UserLoadedAction(arg1, arg2);
		}

		#region IWkWrapper

		public bool IsConnected
		{
			get { return AccessInfo != null && AccessInfo.IsHasToken; }
		}

		public bool IsUserLoaded
		{
			get { return UserProfile != null && UserProfile.uid != int.MinValue && UserProfile.uid > -1; }
		}

		#endregion IWkWrapper

	}
}
