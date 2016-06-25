using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VkDay.vkontakte;
using VKLib;
using VKLib.vkontakte;

namespace VKLib
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			//Test();
		}

		private static void Test()
		{
			VkWrapper wrapper = new VkWrapper();

			wrapper.ShowAutorizationWindow();
			wrapper.AutorizedAction += (s, e) => wrapper.InitUser();

			while (!wrapper.IsUserLoaded)
			{
				
			}

			var iam = CommandsGenerator.ProfileCommands.GetUser(wrapper.UserProfile.uid);
			Console.WriteLine(iam.ToString());
			Console.Read();
		}
	}
}
