using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Threading;
using System.ComponentModel;
namespace VK
{
    public class Downloader
    {
        private string path;
        private WebClient web;
        private DownloadProgressChangedEventHandler OnDownloadProgressChanged;
        private DownloadDataCompletedEventHandler OnDownloadComplete;


        public string GetPatch(){
            return path;
        }

        public Downloader(string path)
        {
            this.path = path;
            web = new WebClient();
        }

        public void SetOnChanged(DownloadProgressChangedEventHandler onChanged)
        {
            this.OnDownloadProgressChanged = onChanged;
        }

        public void SetOnLoaded(DownloadDataCompletedEventHandler onCompleted)
        {
            this.OnDownloadComplete = onCompleted;
        }


        public void Download(string uri,string filename)
        {
            try
            {
                web.DownloadProgressChanged += OnDownloadProgressChanged;
                //web.DownloadDataCompleted += OnDownloadComplete;
                web.DownloadFileAsync(new Uri(uri), path + filename);
                while (true)
                {
                    if (!web.IsBusy) break;
                }
                //IOHandler.OpenPath(path);
                OnDownloadComplete(null, null);
                web.DownloadProgressChanged -= OnDownloadProgressChanged;
                //web.DownloadDataCompleted -= OnDownloadComplete;
            }
            catch (WebException ex)
            {
                System.Windows.Forms.MessageBox.Show(""+ex.Message);
            }
        }

        private void web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /*private void web_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }*/


    }
}
