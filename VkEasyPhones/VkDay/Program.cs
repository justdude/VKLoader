using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VKLib;

namespace VKLib
{
	class Program
	{
		private static void Test()
		{
			APIManager.Instance.Connect();
			APIManager.Instance.InitUser();

			var iam = CommandsGenerator.ProfileCommands.GetUser(APIManager.Instance.Profile.uid);
			Console.WriteLine(iam.ToString());
			Console.Read();
		}
	}
}
