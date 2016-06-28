using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKMusicSync.UserInfo.ViewModel
{
	public class UserInfoViewModel : MIP.MVVM.TabViewModel
	{
		private const string UserInfoText = "Информация";
		
		#region .Ctr

		public UserInfoViewModel():base()
		{
			Header = UserInfoText;
		}

		#endregion



	}
}
