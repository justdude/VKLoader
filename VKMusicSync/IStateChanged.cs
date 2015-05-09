using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VKMusicSync.Delegates;

namespace VKMusicSync
{
	public interface IStateChanged
	{
		void OnLoadStarted(object sender, Argument state);

		void OnProgresChanged(object sender, Argument state);

		void OnLoadEnded(object sender, Argument state);

		void OnRaiseError(object sender, Argument state);

	}
}
