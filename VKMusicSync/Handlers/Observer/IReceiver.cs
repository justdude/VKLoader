using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKMusicSync.Handlers.Observer
{
	public interface IReceiver
	{
		
		void HandleRequest(object sender);
	}
}
