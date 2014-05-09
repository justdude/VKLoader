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
        protected XmlDocument Result {get; set;}
        
        public DownloadProgressChangedEventHandler OnCommandExecuting{get; set;}

        public BaseCommand(string CommandName, NameValueCollection Params)
        {
            this.AccessData = APIManager.AccessData;
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
            return String.Format("https://api.vkontakte.ru/method/{0}.xml?access_token={1}&{2}",
                                    name,
                                    accessToken,
                                    String.Join("&", SelectItem(param)));
        }

        private string[] SelectItem(NameValueCollection qs)
        {
            //from item in Params.AllKeys select item + "=" + Params[item]
            List<string> str = new List<string>();
            foreach (string obj in qs.AllKeys)
                str.Add(obj + "=" + qs[obj]);
            return str.ToArray();
        }

        protected void ExecuteCommand()
        {
            this.Result = new XmlDocument();
            System.Net.WebClient downloader = new System.Net.WebClient();

            string str = MakeQueryString(CommandName, AccessData.AccessToken, Params);

            downloader.DownloadProgressChanged += OnCommandExecuting;
            downloader.DownloadFileAsync(new Uri(str), CommandName);

            while (true)
                if (!downloader.IsBusy) break;
            FileStream fileStream = new FileStream(CommandName, FileMode.Open, FileAccess.Read);
            
            Result.Load(fileStream);

            downloader.DownloadProgressChanged -= OnCommandExecuting;
            downloader.Dispose();
            fileStream.Dispose();
        }


        public abstract List<T> Execute();

        public abstract void ExecuteNonQuery();
    }
}
