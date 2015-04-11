using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MVVM
{
	public class ListTabViewModel<M,VM> : TabModelView
	{
		protected List<M> ItemsData;
		public ObservableCollection<VM> Items {get; private set;}
		
		//new ObservableCollection<SoundModelView>();

		public ListTabViewModel():base()
		{ 
			Items = new ObservableCollection<VM>();
		}

		protected void FillFromData(Func<M, VM> creator)
		{
			if (creator == null)
				return;

			Items.Clear();

			foreach (M item in ItemsData)
			{
				var viewModel = creator(item);

				if (viewModel == null)
					continue;

				Items.Add(viewModel);
			}
		}

	}
}
