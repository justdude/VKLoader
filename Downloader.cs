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
        string path;


        public string GetPatch(){
            return path;
        }

        public delegate void OnDownloading(object sender, DownloadProgressChangedEventArgs e);
        public delegate void OnCompleted(object sender, AsyncCompletedEventArgs e);
        System.Net.WebClient web;

        public Downloader(string path)
        {
            this.path = path;
            web = new System.Net.WebClient();
            /*
             * web.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(onDownloading);
            web.DownloadFileCompleted += new AsyncCompletedEventHandler(onDownload);*/
        }


        public void Download(string uri,string filename)
        {
            try
            {
                web.DownloadFile(new Uri(uri), path + filename);
            }
            catch (WebException ex)
            {
                System.Windows.Forms.MessageBox.Show(""+ex.Message);
            }
        }


    }
}
