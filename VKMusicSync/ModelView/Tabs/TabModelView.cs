using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using VKMusicSync.Interfaces;

namespace VKMusicSync.ModelView.Tabs
{

	public static class Constants
	{
		public const string VOID = "VOID";
		public const string VkAudio = "VkAudio";
		public const string VkRecommendations = "VkRecommendations";
		public const string VkPopular = "VkPopular";


		public static readonly Dictionary<string, string> Tranclates = new Dictionary<string, string>()
		{
			{ VOID, "Пустая" },
			{ VkAudio, "Аудиозаписи" },
			{ VkRecommendations, "Рекомендации" },
			{ VkPopular, "Популярные" }

		};
	}

	public class TabModelView : ViewModelBase
	{
		public string Header
		{
			get
			{
				if (string.IsNullOrEmpty(TypeName) || !Constants.Tranclates.ContainsKey(TypeName))
					return "";

				return Constants.Tranclates[TypeName];
			}
		}
		public string TypeName { get; set; }

		public TabModelView()
		{
			TypeName = Constants.VOID;
		}
		public TabModelView(string type)
		{
			TypeName = type;
		}

	}

	public class VKAudioTabModelView: TabModelView
	{
		//public ISoundListModelView Child { get; set; }

		public VKAudioTabModelView() : base()
		{
		//	Child = new SoundListModelView();
		}

		public VKAudioTabModelView(string type) : base(type)
		{
		//	Child = new SoundListModelView();
		}
	}



}
