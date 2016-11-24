using VKMusicSync.Constants;

namespace VKMusicSync.ViewModel
{
	public class UserInfoViewModel : MIP.MVVM.TabViewModel
	{
		#region .Ctr

		public UserInfoViewModel():base()
		{
			Header = Translates.UserInfoText;
		}

		#endregion



	}
}
