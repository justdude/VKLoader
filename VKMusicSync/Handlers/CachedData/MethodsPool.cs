using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKMusicSync.Handlers.CachedData
{
	public class MethodsPool
	{
		private static MethodsPool mvInstance;

		public static MethodsPool Instance
		{
			get
			{
				return mvInstance;
			}
		}

		static MethodsPool()
		{
			mvInstance = new MethodsPool();
		}


	}
}
