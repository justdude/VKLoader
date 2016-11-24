using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKLib.Delegates;
using VKMusicSync.Delegates;

namespace VKMusicSync.Handlers.Synchronize
{
	public partial class SynhronizingProvider<T>
	{
		private void UploadFile(IDownnloadedData value, HandleDataEvent OnUploadAction)
		{
			ActionsDelegates.Execute((value as IStateChanged).OnLoadStarted,
						 this,
						 new Argument() { result = false });


			if (OnUploadAction != null)
				OnUploadAction(value);


			ActionsDelegates.Execute((value as IStateChanged).OnLoadEnded,
										this,
										new Argument() { result = true });

			/* var downloader = new DataLoader(this.Path);
			 downloaders.Add(downloader);

			 downloader.OnUploadComplete += (r, e) =>
			 {
				 ActionsDelegates.Execute(value.InstanceWithEvents.OnLoadEnded,
										  this,
										  new Delegates.Argument() { result = true });

				 OnDownloadCompleteActions(value, downloader);

				 if (this.OnProgress != null)
					 OnProgress(this,
								new ProgressArgs(1, CountLoadedFiles / (double)FilesCount,
								0,
								null));
			 };

			 ActionsDelegates.Execute(value.InstanceWithEvents.OnLoadStarted,
									  this,
									  new Delegates.Argument() { result = false });
			 */
			//downloader.UploadAsync(value.Path, value.FileName + " " + value.FileExtention);

		}

	}
}
