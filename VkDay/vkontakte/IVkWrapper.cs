using System;
using VKLib;

namespace VkDay.vkontakte
{
	public interface IVkWrapper
	{
		bool IsConnected { get; }

		bool IsUserLoaded { get; }

		void ShowAutorizationWindow();

		void InitUser();

		event Action<object, VKApi.ConnectionState> AutorizedAction;

	}
}
