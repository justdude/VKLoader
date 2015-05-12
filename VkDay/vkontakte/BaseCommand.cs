using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace vkontakte
{
    public abstract class BaseCommand<T> where T: class
    {

        public AccessData AccessData {get; set;}
        public string CommandName {get; set;}
        public NameValueCollection Params {get; set;}
        protected XmlDocument xmlRes {get; set;}
        
        public DownloadProgressChangedEventHandler OnCommandExecuting{get; set;}

        private string queryString;
        public string QueryString
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
            this.AccessData = APIManager.Instance.AccessData;
            this.CommandName = CommandName;
            this.Params = Params;
        }

        public BaseCommand(AccessData AccessData,string CommandName, NameValueCollection Params)
        {
            this.AccessData = AccessData;
            this.CommandName = CommandName;
            this.Params = Params;
        }

        private string MakeQueryString(string name, string accessToken, NameValueCollection param)
        {
            var cachedStr = String.Format("https://api.vk.com/method/{0}.xml?{1}&access_token={2}",
                                    name,
                                     String.Join("&", SelectItem(param)),
                                    accessToken
                                   );

            return cachedStr;
        }

        public string GetParamsWithToken()
        {
            var cachedStr = String.Format("{0}&access_token={1}",
                         String.Join("&", SelectItem(Params)),
                         this.AccessData.AccessToken   );
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
    }
}
