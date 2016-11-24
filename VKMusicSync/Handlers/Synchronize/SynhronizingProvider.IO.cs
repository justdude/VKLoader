using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKMusicSync.Handlers.Synchronize
{
	public partial class SynhronizingProvider<T>
    {
		private bool AudioComparer<T>(System.IO.FileInfo[] files, T value) where T : IDownnloadedData
		{
			string valueFileName = value.FileName + " " + value.FileExtention;
			return Array.Exists<System.IO.FileInfo>(files, p => (IOHandler.ParseFileName(p) == valueFileName));
		}

    }
}
