using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using VKSyncMusic.Handlers.Synchronize;
using VKSyncMusic.Model;
using VKSyncMusic.ModelView;

namespace VKSyncMusic.Interfaces
{
	public interface ISoundListModelView
	{
		bool IsSyncing { get; }
		List<Sound> SoundsData { get; }
		ObservableCollection<SoundModelView> Sounds { get; }

		SynhronizeProcessor<Sound> Processor { get; set; }

		void UpdateList();
		void SyncData();
		void CancelSync();
		void CheckAll(bool state);
	}



}
