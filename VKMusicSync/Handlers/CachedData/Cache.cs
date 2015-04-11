using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKMusicSync.Handlers.CachedData
{
	public static class Cache
	{
		public static Dictionary<object, object> Items = new Dictionary<object, object>();

		public static void AddIfNotExist(object key, object value)
		{
			if (Items.ContainsKey(key))
				return;

			Items.Add(key, value);
		}

		public static object Get(object key)
		{
			if (Items.ContainsKey(key))
				return Items[key];

			return null;
		}
	}
}
