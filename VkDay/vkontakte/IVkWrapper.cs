using System;
using VKLib;
using VKLib.Model;

namespace VkDay.vkontakte
{
	public interface IVkWrapper
	{
		VKApi VkInstance { get;  }
		Profile UserProfile { get; }
		bool IsConnected { get; }

		bool IsUserLoaded { get; }

		void ShowAutorizationWindow();

		void InitUser();
		void Clear();

		event Action<object, VKApi.ConnectionState> AutorizedAction;
		event Action<object, VKApi.ConnectionState> UserLoadedAction;

	}
}
