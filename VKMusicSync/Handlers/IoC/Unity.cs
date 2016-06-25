using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Microsoft.Practices.Unity;

namespace VKMusicSync.Handlers.IoC
{
	public static class Unity
	{
		private static object modSync = new object();
		private static UnityContainer mvContainer;
		
		public static UnityContainer Instance
		{
			get
			{
				if (mvContainer == null)
				{
					lock(modSync)
					{
						mvContainer = new UnityContainer();
						return mvContainer;
					}
				}
				return mvContainer;
			}
		}

	}
}
