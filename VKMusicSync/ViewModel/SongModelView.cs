using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using DotLastFm.Models;
using MIP.MVVM;
using VKLib.Delegates;
using VKMusicSync.Constants;
using VKMusicSync.Handlers.CachedData;
using VKMusicSync.Handlers.LastFm;
using VKMusicSync.Handlers.Synchronize;
using VKMusicSync.Model;

namespace VKMusicSync.ViewModel
{
	public class SoundViewModel : AdwancedViewModelBase, IDownnloadedData, IStateChanged
	{
		public static bool FreezeClick;

		#region Fields

		private bool mvIsChecked = true;
		private bool currentProgressVisibility;

		#endregion Fields

		public Sound Sound { get; set; }

		#region Sync States
		public SyncStates State
		{
			get
			{
				return Sound.State;
			}
			set
			{
				Sound.State = value;

				RaisePropertyChanged(() => State);
			}
		}
		#endregion

		#region Logic of view

		public bool Checked
		{
			get
			{
				return mvIsChecked;
			}
			set
			{
				if (mvIsChecked == value)
					return;

				mvIsChecked = value;

				this.RaisePropertyChanged(() => Checked);
			}
		}

		public bool CurrentProgressVisibility
		{
			get
			{
				return currentProgressVisibility;
			}
			set
			{
				if (currentProgressVisibility == value)
					return;

				currentProgressVisibility = value;

				RaisePropertyChanged(() => CurrentProgressVisibility);
			}

		}

		#endregion Logic of view

		#region Sound
		public string Artist
		{
			get { return Sound.artist; }
			set
			{
				if (Sound.artist == value)
					return;

				Sound.artist = value;
				RaisePropertyChanged(() => Artist);
			}
		}

		public string Title
		{
			get { return Sound.title; }
			set
			{
				if (Sound.title == value)
					return;

				Sound.title = value;
				RaisePropertyChanged(() => Title);
			}
		}

		public int Duration
		{
			get
			{
				return Sound.duration;
			}
			set
			{
				if (Sound.duration == value)
					return;

				Sound.duration = value;
				RaisePropertyChanged(() => Duration);
			}
		}

		public int Quality
		{
			get { return 0; }
			set
			{
				//Sound.Quality = value;
				RaisePropertyChanged(() => Quality);
			}
		}

		public double Size
		{
			get { return Sound.Size; }
			set
			{
				if (Math.Abs(Sound.Size - value) < Const.Prec)
					return;

				Sound.Size = value;

				RaisePropertyChanged(() => Size);
			}
		}

		public double LoadedSize
		{
			get { return Sound.LoadedSize; }
			set
			{
				if (Math.Abs(Sound.Size - value) < Const.Prec)
					return;

				Sound.LoadedSize = value;

				RaisePropertyChanged(() => LoadedSize);
			}
		}



		/* public string PhotoPath
		 {
			 get 
			 {
				 return Sound.authorPhotoPath; 
			 }
			 set
			 {
				 if (value!=Sound.authorPhotoPath)
				 {
					 Sound.authorPhotoPath = value;
					 OnPropertyChanged("PhotoPath");
				 }
			 }
		 }*/


		private BitmapImage mvImage = null;

		public BitmapImage Photo
		{
			get
			{
				if (mvImage == null)
				{
					if (!string.IsNullOrWhiteSpace(Sound.authorPhotoPath))
					{
						LoadImage();
					}
					//else
					//{
					//	mvImage = InstancesData.Instance.SoundAuthor;
					//}

				}
				return mvImage;
			}
			set
			{
				if (Equals(value, mvImage))
					return;

				mvImage = value;

				RaisePropertyChanged(() => LoadedSize);
			}
		}

		private void LoadImage()
		{
			BitmapImage img = Cache.Get(Sound.artist) as BitmapImage;

			if (img != null)
			{
				mvImage = img;
			}

			try
			{
				CurrentDispatcher.BeginInvoke(new Action(() =>
					mvImage = new BitmapImage(new Uri(Sound.authorPhotoPath))), null);
				Cache.AddIfNotExist(Sound.artist, mvImage);

			}
			catch (Exception)
			{
				mvImage = InstancesData.Instance.SoundAuthor;
			}
		}

		public bool IsLoadedToDisk
		{
			get { return Sound.IsLoadedToDisk; }
			set
			{
				if (Sound.IsLoadedToDisk != value)
				{
					Sound.IsLoadedToDisk = value;
					RaisePropertyChanged(() => IsLoadedToDisk);

					// VKMusicSync.ViewModel.SoundDownloaderMovelView.Instance.UpdateList();
				}
			}
		}
		//SimilarArtist


		public List<string> SimilarArtist
		{
			get { return Sound.similarArtists; }
			set
			{
				Sound.similarArtists = value;

				RaisePropertyChanged(() => SimilarArtist);
			}
		}

		public List<string> Albums
		{
			get { return Sound.Albums; }
			set
			{
				if (Sound.Albums == value)
					return;

				Sound.Albums = value;

				RaisePropertyChanged(() => Albums);
			}
		}

		#endregion

		#region Ctr.

		public SoundViewModel(Sound sound)
		{
			Sound = sound;
		}

		#endregion Ctr.

		#region Path

		/*public string GenerateFileName()
		{
			return this.Sound.GenerateFileName();
		}

		public string GenerateFileExtention()
		{
			return this.Sound.GenerateFileExtention();
		}

		public string GetUrl()
		{
			return this.Sound.GetUrl();
		}*/

		#endregion Path

		#region Path2

		public string PathWithFileName
		{
			get { return Sound.PathWithFileName; }
		}

		public string FileName
		{
			get
			{
				return Sound.FileName;
			}
			set
			{
				Sound.FileName = value;
			}
		}

		public string FileExtention
		{
			get
			{
				return Sound.FileExtention;
			}
		}

		public string Path
		{
			get
			{
				return Sound.Path;
			}
			set
			{
				Sound.Path = value;
			}
		}

		public string MD5
		{
			get
			{
				return Sound.MD5;
			}
			set
			{
				Sound.MD5 = value;
			}
		}

		#endregion

		#region Events

		public void OnLoadStarted(object sender, Argument state)
		{
			if (!FreezeClick)
				FreezeClick = true;

			CurrentProgressVisibility = true;
		}

		public void OnProgresChanged(object sender, Argument state)
		{

		}

		public async void OnLoadEnded(object sender, Argument state)
		{
			CurrentProgressVisibility = false;

			bool result = (bool)state.result;
			if (result)
			{
				State = SyncStates.Synced;
			}

			Checked = !((bool)state.result);
			IsLoadedToDisk = (bool)state.result;

			await Task.Run(() => LoadTagsFromLast(Sound));
		}

		public void OnRaiseError(object sender, Argument state)
		{

		}

		#endregion Events

		private void LoadTagsFromLast(object soundObj)
		{
			var sound = soundObj as Sound;
			if (sound == null) return;

			ArtistWithDetails artist = null;
			try
			{
				artist = LastFmHandler.Api.Artist.GetInfo(sound.artist);
				sound.authorPhotoPath = artist.Images[2].Value; // little spike 
			}
			catch
			{
				// ignored
			}

			if (artist == null)
				return;

			sound.similarArtists = artist.SimilarArtists.Select(el => el.Name).ToList();
		}

		public bool IsEqual(IDownnloadedData data)
		{
			return Sound.IsEqual(data);
		}
	}
}
