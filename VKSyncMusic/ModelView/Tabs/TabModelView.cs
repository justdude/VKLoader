using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using VKSyncMusic.Handlers.Synchronize;
using VKSyncMusic.Interfaces;

namespace VKSyncMusic.ModelView.Tabs
{
	public class TabModelView : ViewModelBase
	{
		public ISoundListModelView SoundList { get; set; }

		public Constants.TabTypes TypeName { get; set; }

		public string Header
		{
			get
			{
				if (!Constants.Tranclates.ContainsKey(TypeName))
					return "";

				return Constants.Tranclates[TypeName];
			}
		}

		#region Ctr
		public TabModelView(): base()
		{
			TypeName = Constants.TabTypes.VOID;
		}
		public TabModelView(Constants.TabTypes type):base()
		{
			TypeName = type;
		}

		public override string ToString()
		{
			return Header;
		}
		#endregion

		#region Methods

		public virtual void UpdateList(SynhronizeProcessor<Model.Sound> processor, ISoundListModelView target)
		{
			SoundList = target;
			SoundList.Processor = processor;
			SoundList.UpdateList();
		}

		#endregion
	}

	public class VKAudioTabModelView: TabModelView
	{
		//public ISoundListModelView Child { get; set; }

		public VKAudioTabModelView() : base()
		{
		//	Child = new SoundListModelView();
		}

		public VKAudioTabModelView(Constants.TabTypes type) : base(type)
		{
		//	Child = new SoundListModelView();
		}
	}



}
