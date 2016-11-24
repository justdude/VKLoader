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
		private void OnDownloadProgressChanged(IStateChanged value)
		{
			ActionsDelegates.Execute(value.OnProgresChanged,
				this, new Argument() { result = false });

			if (this.OnProgress != null)
				OnProgress(this,
					new ProgressArgs(CountLoadedFiles , FilesCount));
		}
		private void OnDownloadComplete(IStateChanged value)
		{
			ActionsDelegates.Execute(value.OnLoadEnded,
				this,
				new Argument() { result = true });

			if (this.OnProgress != null)
				OnProgress(this, 
					new ProgressArgs(CountLoadedFiles, FilesCount));
		}

	}
}
