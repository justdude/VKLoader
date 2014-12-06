using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.ComponentModel;

namespace VKSyncMusic.Handlers
{

    public class DataLoader
    {
        public string Path
        {
            get;
            private set;
        }
        private WebClient modLoader
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

        public UploadProgressChangedEventHandler OnUploadProgressChanged
        {
            get;
            set;
        }
        public UploadFileCompletedEventHandler OnUploadComplete
        {
            get;
            set;
        }

        public DataLoader(string path)
        {
            this.Path = path;
            modLoader = new WebClient();
        }


        public void Download( string uri, string filename)
        {
            try
            {
                modLoader.DownloadProgressChanged += OnDownloadProgressChanged;
                modLoader.DownloadFileCompleted   += OnDownloadComplete;
                modLoader.DownloadFile(new Uri(uri), Path + filename);
                
                modLoader.DownloadProgressChanged -= OnDownloadProgressChanged;
                //modLoader.DownloadDataCompleted -= OnDownloadCompleteActions;
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
                modLoader.DownloadProgressChanged += OnDownloadProgressChanged;
                modLoader.DownloadFileCompleted += OnDownloadComplete;
                
                modLoader.DownloadFileAsync(new Uri(uri), Path + filename);
               /* while (true)
                {
                    if (!modLoader.IsBusy) break;
                }*/

                //modLoader.DownloadProgressChanged -= OnDownloadProgressChanged;
                //modLoader.DownloadDataCompleted -= OnDownloadCompleteActions;
            }
            catch (WebException ex)
            {
                //System.Windows.Forms.MessageBox.Show(""+ex.Message); //SPIKE!!!!!!!
            }
        }


        public void Upload(string uri, string filename)
        {
            try
            {
                modLoader.UploadProgressChanged += OnUploadProgressChanged;
                modLoader.UploadFileCompleted += OnUploadComplete;
                modLoader.UploadFile(new Uri(uri), Path + filename);

                modLoader.DownloadProgressChanged -= OnDownloadProgressChanged;
                //modLoader.DownloadDataCompleted -= OnDownloadCompleteActions;
            }
            catch (WebException ex)
            {
                //System.Windows.Forms.MessageBox.Show(""+ex.Message); //SPIKE!!!!!!!
            }
        }

        public void UploadAsync(string uri, string filename)
        {
            try
            {
                modLoader.UploadProgressChanged += OnUploadProgressChanged;
                modLoader.UploadFileCompleted += OnUploadComplete;

                modLoader.UploadFileAsync(new Uri(uri), Path + filename);
            }
            catch (WebException ex)
            {
                //System.Windows.Forms.MessageBox.Show(""+ex.Message); //SPIKE!!!!!!!
            }
        }


        public void CancelAsync()
        {
            modLoader.CancelAsync();
        }

        ~ DataLoader()
        {
            this.modLoader.Dispose();
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
