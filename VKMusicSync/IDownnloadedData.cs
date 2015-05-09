using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VKMusicSync.Handlers.Synchronize;
using VKMusicSync.Model;

namespace VKMusicSync
{
	public interface IDownnloadedData : IData
	{
		IStateChanged InstanceWithEvents { get; }

		double LoadedSize { get; set; }
		bool IsLoadedToDisk { get; set; }
		SyncStates State { get; set; }

		bool IsEqual(IDownnloadedData data);
	}
}
