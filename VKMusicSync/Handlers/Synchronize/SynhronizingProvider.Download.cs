using System.Threading;
using System.Threading.Tasks;
using VKLib.Delegates;
using VKMusicSync.Delegates;

namespace VKMusicSync.Handlers.Synchronize
{
	public partial class SynhronizingProvider<T>
	{
		private void DownloadFile(IDownnloadedData value)
		{
			var downloader = new DataLoader(Path);

			downloader.OnDownloadProgressChanged += (r, e) =>
				OnDownloadProgressChanged(value as IStateChanged);

			downloader.OnDownloadComplete += (r, e) =>
				OnDownloadComplete(value as IStateChanged);

			ActionsDelegates.Execute((value as IStateChanged).OnLoadStarted,
									 this,
									 new Argument() { result = false });
			lock (typeof(DataLoader))
			{
				var options = new ParallelOptions();
				options.MaxDegreeOfParallelism = 4;
				options.CancellationToken = CancellationToken.None;

				DataLoader.Download(value.Path, value.FileName + " " + value.FileExtention, options, null);
			}
			
			//downloader.Download(value.Path, value.FileName + " " + value.FileExtention);

			downloader.Dispose();
		}

	}
}
