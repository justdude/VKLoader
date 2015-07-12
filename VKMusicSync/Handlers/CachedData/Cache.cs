using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace VKMusicSync.Handlers.CachedData
{
	public static class Cache
	{
		public static readonly Dictionary<object, object> Items = new Dictionary<object, object>();

		//public static readonly Dictionary<object, string> ItemsPath = new Dictionary<object, object>();

		private static readonly object modSyncObject = new object();


		public static void AddIfNotExist(object key, object value)
		{
			lock (modSyncObject)
			{
				if (Items.ContainsKey(key))
					return;

				Items.Add(key, value);
			}
		}

		public static void Serialize()
		{
			lock (modSyncObject)
			{

			}
		}

		public static object Get(object key)
		{
			lock (modSyncObject)
			{
				if (Items.ContainsKey(key))
					return Items[key];
			}
			return null;
		}

		static void Test()
		{
		}
	}
}
