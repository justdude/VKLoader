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

namespace VKMusicSync.UserInfo.View
{
	/// <summary>
	/// Interaction logic for ctrUserInfo.xaml
	/// </summary>
	public partial class ctrUserInfo : ControlExtended
	{
		public ctrUserInfo()
		{
			InitializeComponent();
			Loaded += ctrUserInfo_Loaded;
		}

		void ctrUserInfo_Loaded(object sender, RoutedEventArgs e)
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
