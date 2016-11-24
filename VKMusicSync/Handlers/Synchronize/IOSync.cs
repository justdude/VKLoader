using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VKMusicSync.Model;

namespace VKMusicSync.Handlers.Synchronize
{
	public class IoSync<T> where T : IDownnloadedData
	{
		private List<T> skippedFiles;
		private readonly DirectoryInfo Path;

		private FileInfo[] mvFolderFiles;
		public FileInfo[] FilesInfo
		{
			get
			{
				return mvFolderFiles ?? Path.GetFiles();
			}
			private set
			{
				mvFolderFiles = value;
			}
		}

		public List<T> ExistData
		{
			get;
			private set;
		}

		public IoSync(string path, string fileExtension)
		{
			this.ExistData = new List<T>();
			this.Path = new DirectoryInfo(path);
			this.skippedFiles = new List<T>();
			this.modFileException = fileExtension;
		}

		public void UpdateData()
		{
			if (!Path.Exists)
			{
				Path.Create();
			}
			mvFolderFiles = Path.GetFiles(modFileException, SearchOption.AllDirectories);
		}

		public void CompareFolderFiles(List<T> containValues, Func<FileInfo[], T, bool> Check)
		{
			for (int i = 0; i < containValues.Count; i++)
				if (!Check(FilesInfo, containValues[i]) && !ExistData.Contains(containValues[i]))
					ExistData.Add(containValues[i]);
		}

		public List<T> CastExistData(Func<T> Creator)
		{
			return DataHelper<T>.CastInfoToList(FilesInfo, Creator);
		}

		public string modFileException { get; set; }
	}
}
