using System;
using System.Text.RegularExpressions;

namespace VKLib
{
	public class AccessDataInfo
	{
		#region Properties

		public int UserId { get; private set; }
		public string AccessToken { get; private set; }

		public bool IsHasToken { get { return string.IsNullOrWhiteSpace(AccessToken); } }
		public bool IsHasUserId { get { return UserId >= 0; } }

		#endregion Properties

		#region  Ctr.

		public AccessDataInfo(int userId, string accessToken)
		{
			Reset();

			this.UserId = userId;
			this.AccessToken = accessToken;
		}

		public AccessDataInfo(string url)
		{
			Reset();

			int userId = int.MinValue;
			string accessToken = null;

			Parse(url, out accessToken, out userId);

			this.AccessToken = accessToken;
			this.UserId = userId;
		}

		#endregion Ctr.

		#region Methods

		private void Reset()
		{
			UserId = int.MinValue;
			AccessToken = null;
		}

		public static void Parse(string url, out string AccessToken, out int UserId)
		{
			AccessToken = null;
			UserId = int.MinValue;

			Regex reg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)",
					  RegexOptions.IgnoreCase | RegexOptions.Singleline);

			try
			{
				var mathes = reg.Matches(url);

				foreach (Match m in mathes)
				{
					switch (m.Groups["name"].Value)
					{
						case "access_token":
							AccessToken = m.Groups["value"].Value;
							break;
						case "user_id":
							UserId = int.Parse(m.Groups["value"].Value);
							break;
					}
				}
			}
			catch (Exception ex)
			{
				AccessToken = null;
				UserId = int.MinValue;

				throw;
			}
		}

		#endregion Methods
	}
}
