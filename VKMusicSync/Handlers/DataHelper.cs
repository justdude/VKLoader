using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VKMusicSync.Model;

namespace VKMusicSync.Handlers
{
	public static class DataHelper<TType> where TType : IDownnloadedData
	{

		public static void ConvertFrom<T>(FileInfo fileInfo, T element) where T : IDownnloadedData
		{
			element.Path = fileInfo.DirectoryName;
			//.FileExtention = fileInfo.Extension;
			element.FileName = fileInfo.Name.Replace(fileInfo.Extension, "");
			element.MD5 = fileInfo.GetHashCode().ToString();
			element.IsLoadedToDisk = false;
			element.State = Synchronize.SyncStates.Unknown;
			element.LoadedSize = (double)fileInfo.Length;
		}

		public static List<TType> CastInfoToList(FileInfo[] filesInfo, Func<TType> creator)
		{
			if (filesInfo == null && creator != null)
				return null;

			List<TType> tempColl = new List<TType>();
			TType element = creator();

			foreach (var folderInfo in filesInfo)
			{
				if (element == null) 
					continue;

				ConvertFrom<TType>(folderInfo, element);

				tempColl.Add(element);
			}
			return tempColl;
		}
	}
}
