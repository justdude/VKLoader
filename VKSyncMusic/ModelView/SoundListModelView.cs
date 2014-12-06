using MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VKSyncMusic.Handlers;
using VKSyncMusic.Handlers.Synchronize;
using VKSyncMusic.Interfaces;
using VKSyncMusic.Model;
using VKSyncMusic.MVVM.Collections;
using vkontakte;

namespace VKSyncMusic.ModelView
{

	public class SoundListModelView : ViewModelBase, ISoundListModelView
	{

        #region Fields
				private double progressPercentage = 0;

				private bool isSyncing = false;
				private List<Sound> modSoundsData;
				private ObservableCollection<SoundModelView> mvSounds;

				#endregion

				#region Properties

				public bool IsAllChecked { get; private set; }
				public bool IsSyncing
				{
					get
					{
						return isSyncing;
					}
					set
					{
						if (isSyncing == value)
							return;

						isSyncing = value;

						OnPropertyChanged("IsSyncing");
					}
				}

				public string Type
				{
					get
					{
						return mvType;
					}
					set
					{
						if (mvType == value)
							return;

						mvType = value;

						OnPropertyChanged("Type");
					}
				}

				public List<Sound> SoundsData
				{
					get
					{
						return modSoundsData;
					}
					private set
					{
						modSoundsData = value;
					}
				}

				public ObservableCollection<SoundModelView> Sounds
				{
					get
					{
						return mvSounds;
					}
					private set
					{
						mvSounds = value;
					}
				}

				public SynhronizeProcessor<Sound> Processor
				{
					get;
					set;
				}

				#endregion

        #region Constructor

				public SoundListModelView()
        {
						SoundsData = new List<Sound>();
            Sounds = new ObservableCollection<SoundModelView>();

						//Target = new ObservableCollection<SoundModelView>(sounds.Select(s => new SoundModelView(s)));
						//Sounds.Add(new SoundModelView(new Sound("aaaa", "bbbb")));
        }

        #endregion

				#region Check items

				//private bool CheckIsLoaded()
				//{
				//	if (this.Target != null)
				//		if (this.Target.Count > 0)
				//			return true;
				//	return false;
				//}    
				//private void OnCheckedAllClick()
				//{
            
				//		if (IsAllChecked)
				//		{
				//				IsAllChecked = false;
				//				foreach (var value in Target)
				//						value.Checked = false;
				//				this.CheckedText = "Выбрать все";
                
				//		}
				//		else
				//		{
				//				IsAllChecked = true;
				//				foreach (var value in Target)
				//						value.Checked = true;
				//				this.CheckedText = "Отменить все";
				//		}
				//		UpdateList();
				//}

  
        #endregion

        #region Process vk value to forms

        //private void Init(object sender, DoWorkEventArgs e)
        //{
        //    Status = "Загрузка профиля";
        //    LoadProfileInfo();
        //    Status = "Загрузка треков";
        //    LoadAudioInfo();
        //    Status = "Загрузка информации о треках с Last.Fm";
        //    Handlers.ItemHelper.FillLastInfo(CachedSounds, LastFmHandler.Api);
        //    Status = "Размер файла";
        //    Handlers.ItemHelper.FillDataInfo(CachedSounds);
        //}


        #endregion

        #region Items info load

				private Task LoadAudioInfo()
        {
					Task task = new Task(() =>
					{
						SoundsData.Clear();

						foreach (var item in Processor.ComputedFileList)
						{
							SoundsData.Add(item);
						}

						Execute(() =>
							FillFromData(SoundsData));
					});

					task.Start();

					return task;
        }

        private void FillFromData(List<Sound> soundsData)
        {
            Converter<Sound, SoundModelView> converter = new Converter<Sound, SoundModelView>(c => new SoundModelView(c));
            var cached = new ObservableCollection<SoundModelView>(soundsData.ConvertAll<SoundModelView>(converter));
            Sounds.Clear();
            foreach (var item in cached)
            {
                Sounds.Add(item);
            }
        }
        #endregion


        #region Sync audio


				private string mvType;


        private void CancelSync()
        {
					Processor.CancelDownloading();
        }


				//private void UploadItem(IDownnloadedData data)
				//{
				//		UploadItem(data.Path, data.FileName + data.FileExtention);
				//}

				//private void UploadItem(string sourceFolderPath, string fileName)
				//{
				//		AudioUploadedInfo info = CommandsGenerator.AudioCommands.GetUploadServer(sourceFolderPath, fileName);
				//		string answer = CommandsGenerator.AudioCommands.SaveAudio(info);
				//}

        private Task SyncFolderWithVKAsync()
        {
					
					IsSyncing = true;
					VKSyncMusic.ModelView.SoundModelView.FreezeClick = true;

					//Processor = new SynhronizeProcessor<Sound>(@"Sounds\", ".mp3", 8);

					Processor.OnDone += AdapterSyncFolderWithVKAsyncDone;
					Processor.OnProgress += AdapterSyncFolderWithVKAsyncOnProgress;

          IEnumerable<SoundModelView> selected = Sounds.Where(p => p.Checked);

					Task task = new Task(()=>{
						Processor.SyncFolderWithList<SoundModelView>(selected.ToList());
						Execute(()=> VKSyncMusic.ModelView.SoundModelView.FreezeClick = false);
					});

					return task;
        }

        private void AdapterSyncFolderWithVKAsyncOnProgress(object sender, ProgressArgs e)
        {

        }

        private void AdapterSyncFolderWithVKAsyncDone(object sender, ProgressArgs e)
        {
            //Status = SoundHandler.CountLoadedFiles + "/" + this.Target.Count;  

            IOHandler.OpenPath(Properties.Settings.Default.DownloadFolderPath);
						IsBusy = false;
						IsSyncing = false;
						VKSyncMusic.ModelView.SoundModelView.FreezeClick = false;
        }

        #endregion

				void ISoundListModelView.UpdateList()
				{
					if (Processor == null)
						return;

					if (IsBusy)
						return;

					IsBusy = true;

					var task1 = LoadAudioInfo();
					task1.Start();

					IsBusy = false;
				}

				Task SyncTask = null;

				void ISoundListModelView.SyncData()
				{
					if (IsBusy)
						return;

					if (Processor == null)
											return;

					IsBusy = true;

					SyncTask = SyncFolderWithVKAsync();
					SyncTask.Start();

					
				}

				void ISoundListModelView.CancelSync()
				{
					CancelSync();
					//SyncTask.IsCanceled = true;
					IsBusy = false;
				}

				public void CheckAll(bool state)
				{
					//throw new NotImplementedException();
				}
	}
}
