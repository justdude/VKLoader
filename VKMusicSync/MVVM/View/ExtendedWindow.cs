using MVVM;
using MVVM.AttachedProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKMusicSync.MVVM.View
{
	public class WindowExtended : Elysium.Controls.Window
	{
		protected string Token
		{
			get { return ControlBehavior.GetToken(this); }
		}

		public WindowExtended()
		{
			Closing += ControlExtended_UnLoaded;
		}

		private void ControlExtended_UnLoaded(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Clean();
		}

		public void Clean()
		{
			AdwancedViewModelBase viewModel = DataContext as AdwancedViewModelBase;
			if (viewModel == null)
				return;

			viewModel.CleanViewModel();
			viewModel = null;
		}
	}
}
