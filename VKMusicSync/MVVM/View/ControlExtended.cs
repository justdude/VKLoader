using MVVM;
using MVVM.AttachedProperties;
using Probel.Mvvm.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace VKMusicSync.MVVM.View
{
	public class ControlExtended : UserControl
	{
		protected string Token
		{
			get { return ControlBehavior.GetToken(this); }
		}

		public ControlExtended()
		{
			this.Unloaded += ControlExtended_UnLoaded;
		}

		void ControlExtended_UnLoaded(object sender, System.Windows.RoutedEventArgs e)
		{
			AdwancedViewModelBase viewModel = DataContext as AdwancedViewModelBase;
			if (viewModel == null)
				return;

			viewModel.CleanViewModel();
		}

	}
}
