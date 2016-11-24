using System;
using System.Net;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VKMusicSync.Handlers
{
	public class DataLoader : IDisposable
	{
		private bool isDisposed;
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

		public event DownloadProgressChangedEventHandler OnDownloadProgressChanged
		{
			add { modLoader.DownloadProgressChanged += value; }
			remove { modLoader.DownloadProgressChanged -= value; }
		}

		public event AsyncCompletedEventHandler OnDownloadComplete
		{
			add { modLoader.DownloadFileCompleted += value; }
			remove { modLoader.DownloadFileCompleted -= value; }
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


		public void Download(string uri, string filename)
		{
			try
			{
				modLoader.DownloadFile(new Uri(uri), Path + filename);
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
				modLoader.DownloadFileAsync(new Uri(uri), Path + filename);
			}
			catch (WebException ex)
			{
			}
		}


		public void Upload(string uri, string filename)
		{
			try
			{
				//modLoader.UploadProgressChanged += OnUploadProgressChanged;
				//modLoader.UploadFileCompleted += OnUploadComplete;
				modLoader.UploadFile(new Uri(uri), Path + filename);
			}
			catch (WebException)
			{
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

		private static Action<object, ProgressArgs> DefaulAction = (x, y) => { };

		public static void Download(string uri, string filepath, ParallelOptions options, Action<object, ProgressArgs> stateChanged)
		{
			stateChanged = stateChanged ?? DefaulAction;
			FileStream outStream = null;

			try
			{
				outStream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
				var fileReq = WebRequest.CreateHttp(uri);
				WebResponse resp = fileReq.GetResponse();

				using (var inStream = resp.GetResponseStream())
				{
					var args = new ProgressArgs();
					var length = resp.ContentLength;

					int offset = 1;
					int bufferSize = (int)(length / (double)options.MaxDegreeOfParallelism);
					long iterrations = length / bufferSize;

					Parallel.For(0, iterrations, options,
						(i, state) =>
						{
							if (offset <= 0)
								return;

							byte[] buffer = new byte[bufferSize];
							int expectedOffset = bufferSize * (int)i;

							var currFileRec = WebRequest.CreateHttp(uri);
							currFileRec.AddRange(expectedOffset, expectedOffset + bufferSize);
							var stream = currFileRec.GetResponse().GetResponseStream();

							int readedBytes = stream.Read(buffer, 0, bufferSize);

							if (readedBytes <= 0)
								state.Stop();

							Interlocked.Add(ref offset, readedBytes);

							outStream.Write(buffer, 0, readedBytes);
							outStream.Flush();

							GetValue(args, offset, length);
							stateChanged(inStream, args);

							stream.Dispose();
						});
				}//stream


			}//try
			finally
			{
				if (outStream != Stream.Null)
				{
					outStream.Close();
					outStream.Dispose();
				}
			}
		}

		private static int Offset(short bufferSize, int offset, Stream inStream, FileStream outStream)
		{
			byte[] buffer = new byte[bufferSize];
			offset = inStream.Read(buffer, offset, bufferSize);
			outStream.Write(buffer, offset, buffer.Length);
			outStream.Flush();
			return offset;
		}

		private static void GetValue(ProgressArgs args, double current, double total)
		{
			args.Current = current;
			args.Total = total;
		}

		#region IDisposable

		protected virtual void Dispose(bool isDisposing)
		{
			if (isDisposed)
				return;

			if (isDisposing)
			{
				if (modLoader != null)
				{
					modLoader.UploadProgressChanged -= OnUploadProgressChanged;
					modLoader.UploadFileCompleted -= OnUploadComplete;
					modLoader.CancelAsync();
					modLoader.Dispose();
				}
			}

			modLoader = null;

			isDisposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~DataLoader()
		{
			Dispose(false);
		}

		#endregion IDisposable
	}
}
