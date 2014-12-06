using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKSyncMusic
{
	public static class Constants
	{
		public const string VOID = "VOID";
		public const string VkAudio = "VkAudio";
		public const string VkRecommendations = "VkRecommendations";
		public const string VkPopular = "VkPopular";


		public static readonly Dictionary<TabTypes, string> Tranclates = new Dictionary<TabTypes, string>()
		{
			{ TabTypes.VOID, "Пустая" },
			{ TabTypes.VkAudio, "Аудиозаписи" },
			{ TabTypes.VkRecommendations, "Рекомендации" },
			{ TabTypes.VkPopular, "Популярные" }

		};

		public enum TabTypes
		{
			VOID,
			VkAudio,
			VkRecommendations,
			VkPopular
		}

	}
}
