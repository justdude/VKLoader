using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.ComponentModel;

namespace VKMusicSync.Handlers
{

    public class ProgressArgs
    {
        /// <summary>
        /// Возвращает максимальное число полученных байтов.
        /// </summary>
       public double BytesReceived	
       {
           get;
           private set;
       }
        /// <summary>
        /// Возвращает процент выполнения асинхронной задачи
        /// </summary>
        public double ProgressPercentage
        {
            get;
            private set;
        }
        /// <summary>
        /// bозвращает общее количество байтов в операции по загрузке данных
        /// </summary>
        public double TotalBytesToReceive
        {
            get;
            private set;
        }

        public object UserState
        {
            get;
            private set;
        }

        public ProgressArgs()
        {

        }

        public ProgressArgs(double received, double progressPercentage, double totalBytes, object userState)
        {
            this.BytesReceived = received;
            this.ProgressPercentage = progressPercentage;
            this.TotalBytesToReceive = totalBytes;
            this.UserState = userState;
        }

    }

    public class Downloader
    {
        public string Path
        {
            get;
            private set;
        }
        private WebClient web
        {
            get;
            set;
        }

        /*public delegate void DownloadProgressChangedEvent(Object sender, ProgressArgs e);

        public DownloadProgressChangedEvent OnDownloadProgressChanged
        {
            get;
            set;
        }
        private ProgressArgs args
        {
            get;
            set;
        }*/

        public DownloadProgressChangedEventHandler OnDownloadProgressChanged
        {
            get;
            set;
        }
        public AsyncCompletedEventHandler OnDownloadComplete
        {
            get;
            set;
        }

        public Downloader(string path)
        {
            this.Path = path;
            web = new WebClient();
        }


        public void Download( string uri, string filename)
        {
            try
            {
                web.DownloadProgressChanged += OnDownloadProgressChanged;
                web.DownloadFileCompleted   += OnDownloadComplete;
                web.DownloadFile(new Uri(uri), Path + filename);
                
                web.DownloadProgressChanged -= OnDownloadProgressChanged;
                //web.DownloadDataCompleted -= OnDownloadComplete;
            }
            catch (WebException ex)
            {
                //System.Windows.Forms.MessageBox.Show(""+ex.Message); //SPIKE!!!!!!!
            }
        }

        public void DownloadAsync(string uri, string filename)
        {
            try
            {
                web.DownloadProgressChanged += OnDownloadProgressChanged;
                web.DownloadFileCompleted += OnDownloadComplete;
                
                web.DownloadFileAsync(new Uri(uri), Path + filename);
               /* while (true)
                {
                    if (!web.IsBusy) break;
                }*/

                //web.DownloadProgressChanged -= OnDownloadProgressChanged;
                //web.DownloadDataCompleted -= OnDownloadComplete;
            }
            catch (WebException ex)
            {
                //System.Windows.Forms.MessageBox.Show(""+ex.Message); //SPIKE!!!!!!!
            }
        }

        public void CancelAsync()
        {
            web.CancelAsync();
        }

        ~ Downloader()
        {
            this.web.Dispose();
        }


        /*public void Download(string uri, string filename)
        {
            FileStream outStream = new FileStream(path + filename, FileMode.Create, FileAccess.Write);
            FileStream inStream = new FileStream(uri, FileMode.Open);

            args = new ProgressArgs(0,0,inStream.Length,"progress");
            short bufferSize = 2048;
            byte[] buffer = new byte[bufferSize];

            int offset = inStream.Read(buffer, 0, bufferSize);
            if (OnDownloadProgressChanged != null)
            {
                args = new ProgressArgs(0, 0, inStream.Length, "progress");
                OnDownloadProgressChanged(this, args);
            }

            while(offset > 0)
            {
                offset = inStream.Read(buffer, offset, bufferSize);
                outStream.Write(buffer, 0, buffer.Length);
                outStream.Flush();

                if (OnDownloadProgressChanged!=null)
                {
                    args = new ProgressArgs(0, 0, inStream.Length, "progress");
                    OnDownloadProgressChanged(this, args);
                }
            }

            if (OnDownloadProgressChanged != null)
            {
                args = new ProgressArgs(0, 0, inStream.Length, "progress");
                OnDownloadProgressChanged(this, args);
            }

            outStream.Close();
            inStream.Close();
        }*/




    }
}
