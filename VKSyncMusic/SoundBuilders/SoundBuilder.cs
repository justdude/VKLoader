using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VKSyncMusic.Handlers;
using VKSyncMusic.Handlers.Synchronize;
using VKSyncMusic.Model;
using vkontakte;

namespace VKSyncMusic.SoundBuilders
{

	public class Processor
	{ 
		private SoundProccesorBuilder currentBuilder;

		public Processor(SoundProccesorBuilder builder)
		{
			Set(builder);
		}

		public void Set(SoundProccesorBuilder builder)
		{
			currentBuilder = builder;
		}

		public SynhronizeProcessor<Sound> GetResult()
		{
			return currentBuilder.GetResult();
		}

		public void Build()
		{
			currentBuilder.CreateNew();
			currentBuilder.Init();
			currentBuilder.ReadDataFromDisk();
			currentBuilder.ReadDataFromWeb();
			currentBuilder.Process();
		}
	}

	public abstract class SoundProccesorBuilder
	{
		protected SynhronizeProcessor<Sound> Target;

		public SoundProccesorBuilder()
		{
			Target = new SynhronizeProcessor<Sound>(Properties.Settings.Default.DownloadFolderPath,
															"*.mp3",
															Properties.Settings.Default.ThreadCountToUse);
		}

		public virtual void CreateNew()
		{

		}

		public SynhronizeProcessor<Sound> GetResult()
		{
			return Target;
		}

		public virtual void Init()
		{

		}
		public virtual void ReadDataFromDisk()
		{

		}

		public virtual void ReadDataFromWeb()
		{

		}

		public virtual void Process()
		{

		}
	}


	public class VkListProccesorBuilder : SoundProccesorBuilder
	{

		public VkListProccesorBuilder():base()
		{ }

		public override void CreateNew()
		{
			Target = new SynhronizeProcessor<Sound>(Properties.Settings.Default.DownloadFolderPath,
																"*.mp3",
																Properties.Settings.Default.ThreadCountToUse);
		}

		public override void Init()
		{
			//SoundHandler.OnDone += AdapterSyncFolderWithVKAsyncDone;
			//SoundHandler.OnProgress += AdapterSyncFolderWithVKAsyncOnProgress;
			Target.OnReadDataInfoEvent += FilFromDiskItem;
		}

		public override void Process()
		{
			Func<Sound> Creator = () => { return new Sound(); };
			Target.ComputeModList(Creator, GetAudioFromUser);

			GetDataFromLast();
		}


		protected void GetDataFromLast()
		{
			var manager = new AsyncTaskManager<Sound>();
			manager.Execute = new AsyncTaskManager<Sound>.ExecuteWork(
			(sound) =>
			{
				try
				{
					var artist = Handlers.LastFmHandler.Api.Artist.GetInfo(sound.artist);
					sound.authorPhotoPath = artist.Images[2].Value; // little spike 
					sound.similarArtists = artist.SimilarArtists.Select(el => el.Name).ToList<string>();

				}
				catch (Exception ex)//catch (DotLastFm.Api.Rest.LastFmApiException ex)
				{

				}
			});
		}

		private void FilFromDiskItem(IDownnloadedData item)
		{
			var sound = item as Sound;
			if (sound != null)
				Handlers.TagReader.Read(item.PathWithFileName, sound);
		}

		//SoundsData = CommandsGenerator.AudioCommands.GetAudioRecomendation(APIManager.vk.UserId, 0, false, 100);
		//SoundsData = CommandsGenerator.AudioCommands.GetAudioPopular(false, 0, 0, 100);
		private List<Sound> GetAudioFromUser()
		{
			int count_ = CommandsGenerator.AudioCommands.GetAudioCount(APIManager.vk.UserId, false);

			if (count_ > 0)
			{
				//CommandsGenerator.AudioCommands.OnCommandExecuting += OnCommandLoading;
				return CommandsGenerator.AudioCommands.GetAudioFromUser(APIManager.vk.UserId, false, 0, count_);
			};
			return new List<Sound>();
		}
	}


	public class VkPopularBuilder: VkListProccesorBuilder
	{

		public override void Process()
		{
			Func<Sound> Creator = () => { return new Sound(); };
			Target.ComputeModList(Creator, GetAudioFromUser);

		}

		//SoundsData = CommandsGenerator.AudioCommands.GetAudioRecomendation(APIManager.vk.UserId, 0, false, 100);
		private List<Sound> GetAudioFromUser()
		{
				//CommandsGenerator.AudioCommands.OnCommandExecuting += OnCommandLoading;
				return CommandsGenerator.AudioCommands.GetAudioPopular(false, 0, 0, 100);
		}

	}

	public class VkReccomendationBuilder : VkListProccesorBuilder
	{

		public override void Process()
		{
			Func<Sound> Creator = () => { return new Sound(); };
			Target.ComputeModList(Creator, GetAudioFromUser);

		}

		private List<Sound> GetAudioFromUser()
		{
			//CommandsGenerator.AudioCommands.OnCommandExecuting += OnCommandLoading;
			return CommandsGenerator.AudioCommands.GetAudioRecomendation(APIManager.vk.UserId, 0, false, 100);
		}

	}


	public class SyncSoundProccesorBuilder : SoundProccesorBuilder
	{
		public override void CreateNew()
		{
			Target = new SynhronizeProcessor<Sound>(Properties.Settings.Default.DownloadFolderPath,
																						"*.mp3",
																						Properties.Settings.Default.ThreadCountToUse);
		}

		public override void Init()
		{
			Target.OnReadDataInfoEvent += new SynhronizerBase.HandleDataEvent(FilFromDiskItem);
			Target.OnUploadAction += new SynhronizerBase.HandleDataEvent(UploadItem);
		}

		private void FilFromDiskItem(IDownnloadedData item)
		{
			var sound = item as Sound;
			if (sound != null)
				Handlers.TagReader.Read(item.PathWithFileName, sound);
		}
		private void UploadItem(IDownnloadedData data)
		{
			UploadItem(data.Path, data.FileName + data.FileExtention);
		}

		private void UploadItem(string sourceFolderPath, string fileName)
		{
			AudioUploadedInfo info = CommandsGenerator.AudioCommands.GetUploadServer(sourceFolderPath, fileName);
			string answer = CommandsGenerator.AudioCommands.SaveAudio(info);
		}

	}

}
