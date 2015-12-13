using MIP.MVVM;
using MIP.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VKMusicSync.VKSync.View
{
	/// <summary>
	/// Interaction logic for ctrVkSync.xaml
	/// </summary>
	public partial class ctrVkSync : ControlExtended
	{
		public ctrVkSync()
		{
			InitializeComponent();
			Loaded += ctrVkSync_Loaded;
		}

		void ctrVkSync_Loaded(object sender, RoutedEventArgs e)
		{
			var viewModel = this.DataContext as AdwancedViewModelBase;
			if (viewModel == null)
			{
				return;
			}

			viewModel.Clean();
			viewModel.Token = Token;
		}
	}
}
