using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using VKMusicSync.Handlers;

namespace VKMusicSync.Tests.Network
{
	[TestFixture]
	class DownloaderTest
	{
		private static string url =
			@"https://cs7-5v4.vk-cdn.net/p5/efdfde8f3a5657.mp3?extra=c9b94Hg4BaIDiQzFcUabRFN8FBUPdgBZ7aVrptRokhysIThY2GHRWhtOCsN5uEn4fcoWK21Jm8CD0X1J0FrEbTu04-J9ofbDxAwJFb9fXX6zCLzm3WLqsGmcVEi8QnS6RhIxzmusBdyJAw";

		private static string filePath = "D:\test.mp3";

		[Test(Description = "GetMetaData method test")]
		public void TestDownloading()
		{
			var options = new ParallelOptions();
			options.MaxDegreeOfParallelism = 4;

			DataLoader.Download(url, filePath, options, null);

			FileAssert.DoesNotExist(filePath);

			//Assert.IsNotNull(actual, "The returned metadata instance is null.");
			//Assert.AreEqual(expected.Name, actual.Name, "Name is not same.");


		}
	}
}
