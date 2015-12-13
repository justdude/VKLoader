using MIP.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIP.Behavior;

namespace VKMusicSync.CustomControls
{
	public class WindowExtended : Elysium.Controls.Window
	{
		protected string Token
		{
			get { return MIP.Behavior.CControlBehavior.GetToken(this); }
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

			viewModel.Clean();
			viewModel = null;
		}
	}
}
