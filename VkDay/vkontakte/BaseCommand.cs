using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace VKLib
{
	public abstract class BaseCommand<T> where T : class
	{

		public AccessDataInfo AccessData { get; set; }
		public string CommandName { get; set; }
		public NameValueCollection Params { get; set; }
		protected XmlDocument xmlRes { get; set; }

		public DownloadProgressChangedEventHandler OnCommandExecuting { get; set; }

		private string queryString;
		public virtual string QueryString
		{
			get
			{
				queryString = MakeQueryString(CommandName, AccessData.AccessToken, Params);
				return queryString;
			}
			set
			{
				queryString = value;
			}
		}

		public BaseCommand(string CommandName, NameValueCollection Params)
		{
			this.AccessData = AccessDataInfo.DummyInfo;//APIManager.Instance.AccessDataInfo;
			this.CommandName = CommandName;
			this.Params = Params;
		}

		public BaseCommand(AccessDataInfo accessData, string CommandName, NameValueCollection Params)
		{
			this.AccessData = accessData;
			this.CommandName = CommandName;
			this.Params = Params;
		}

		private string MakeQueryString(string name, string accessToken, NameValueCollection param)
		{
			const string fullQueryStr = @"https://api.vk.com/method/{0}.xml?{1}&access_token={2}";
			const string queryWitoutTokenStr = @"https://api.vk.com/method/{0}.xml?{1}";

			var cachedStr = string.Empty;

			if (string.IsNullOrWhiteSpace(accessToken))
			{
				cachedStr = String.Format(queryWitoutTokenStr, 
					name, String.Join("&", SelectItem(param)));
			}
			else
			{
				cachedStr = String.Format(fullQueryStr, 
					name, String.Join("&", SelectItem(param)), accessToken);
			}

			return cachedStr;
		}

		public string GetParamsWithToken()
		{
			var cachedStr = String.Format("{0}&access_token={1}",
						 String.Join("&", SelectItem(Params)),
						 this.AccessData.AccessToken);
			return cachedStr;

		}


		private string[] SelectItem(NameValueCollection qs)
		{
			//from item in Params.AllKeys select item + "=" + Params[item]
			List<string> str = new List<string>();
			foreach (string obj in qs.AllKeys)
				str.Add(obj + "=" + qs[obj]);
			return str.ToArray();
		}

		public virtual void ExecuteCommand()
		{
			this.xmlRes = new XmlDocument();
			System.Net.WebClient downloader = new System.Net.WebClient();

			queryString = MakeQueryString(CommandName, AccessData.AccessToken, Params);

			downloader.DownloadProgressChanged += OnCommandExecuting;
			try
			{
				downloader.DownloadFileAsync(new Uri(queryString), CommandName);
			}
			catch
			{ }
			while (true)
				if (!downloader.IsBusy) break;
			FileStream fileStream = new FileStream(CommandName, FileMode.Open, FileAccess.Read);

			xmlRes.Load(fileStream);

			downloader.DownloadProgressChanged -= OnCommandExecuting;
			downloader.Dispose();
			fileStream.Dispose();
		}

		public virtual bool Validate(BaseCommand<T> command)
		{
			return true;
		}
	}
}
