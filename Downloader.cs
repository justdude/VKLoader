using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Threading;
using System.ComponentModel;
namespace VK
{
    class Downloader
    {
        string path;


        public string GetPatch(){
            return path;
        }

        public delegate void OnDownloading(object sender, DownloadProgressChangedEventArgs e);
        public delegate void OnCompleted(object sender, AsyncCompletedEventArgs e);
        System.Net.WebClient web;
        /*OnCompleted onDownload;
        OnDownloading onDownloading;*/

        public Downloader(string path, OnDownloading onDownloading, OnCompleted onDownload)
        {
            this.path = path;
           /* this.onDownloading = onDownloading;
            this.onDownload = onDownload;*/
            web = new System.Net.WebClient();
            web.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(onDownloading);
            web.DownloadFileCompleted += new AsyncCompletedEventHandler(onDownload);
        }

        public void Download(string uri)
        {
            web.DownloadFile(new Uri(uri), path);
        }

        public void Download(string uri,string yourPath)
        {
            web.DownloadFile(new Uri(uri), yourPath);
        }
    
        public void OpenPath(){
            System.Diagnostics.Process.Start(path);
        }


    }
}
