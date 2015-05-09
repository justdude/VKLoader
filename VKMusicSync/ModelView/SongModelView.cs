using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVM;
using VKMusicSync.Model;
using DotLastFm;
using System.Windows.Media.Imaging;
using VKMusicSync.Handlers.Synchronize;
using VKMusicSync.Delegates;
using System.Threading;
using DotLastFm.Models;
namespace VKMusicSync.ModelView
{
	public class SoundModelView : ViewModelBase, IDownnloadedData, IStateChanged
	{

		public IStateChanged InstanceWithEvents
		{
			get
			{
				return this;
			}
		}

		public Sound Sound { get; set; }
		/*
		 public string UserId { get; set; }
		public string artist { get; set; }
		public string title { get; set; }
		public string duration { get; set; }
		public string url { get; set; }
		public string lyrics_id { get; set; }
		public string genre_id { get; set; }
		 */

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
				OnPropertyChanged("State");
			}
		}
		#endregion

		public static bool FreezeClick = false;

		private bool ischecked = true;

		public bool Checked
		{
			get
			{
				return ischecked;
			}
			set
			{
				//if (FreezeClick == false)
				if (ischecked != value)
				{
					ischecked = value;
					OnPropertyChanged("Checked");
				}
			}
		}

		private bool currentProgressVisibility = false;
		public bool CurrentProgressVisibility
		{
			get
			{
				return currentProgressVisibility;
			}
			set
			{
				if (currentProgressVisibility != value)
				{
					currentProgressVisibility = value;
					OnPropertyChanged("CurrentProgressVisibility");
				}
			}

		}

		#region Sound
		public string Artist
		{
			get { return Sound.artist; }
			set
			{
				Sound.artist = value;
				OnPropertyChanged("Artist");
			}
		}

		public string Title
		{
			get { return Sound.title; }
			set
			{
				Sound.title = value;
				OnPropertyChanged("Title");
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
				Sound.duration = value;
				OnPropertyChanged("Duration");
			}
		}

		public int Quality
		{
			get { return 192; }
			set
			{
				//Sound.Quality = value;
				OnPropertyChanged("Duration");
			}
		}

		public double Size
		{
			get { return Sound.Size; }
			set
			{
				Sound.Size = value;
				OnPropertyChanged("Size");
			}
		}

		public double LoadedSize
		{
			get { return Sound.LoadedSize; }
			set
			{
				Sound.LoadedSize = value;
				OnPropertyChanged("LoadedSize");
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
						if (Sound.authorPhotoPath != null && Sound.authorPhotoPath.Length > 0)
						{
							//try
							//{
							//CurrentDispatcher.BeginInvoke(new Action(()=>
							//	mvImage = new BitmapImage(new Uri(Sound.authorPhotoPath))), null);
							//}
							//catch (Exception)
							//{
							//	CurrentDispatcher.BeginInvoke(new Action(() =>
							//		mvImage = new BitmapImage(new Uri(VkDay.Constants.ImageDefaultURI))), null);
							//} 
						}
				}
				//else
				//{
				//	var bm = new BitmapImage(new Uri(Constants.Const.UnjnownAuthorPath));
				//	return bm;
				//}
				return mvImage;
			}
			set
			{
				if (value != mvImage)
				{
					mvImage = value;
					OnPropertyChanged("Photo");
				}
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
					OnPropertyChanged("IsLoadedToDisk");

					// VKMusicSync.ModelView.SoundDownloaderMovelView.Instance.UpdateList();
				}
			}
		}
		//SimilarArtist


		public List<string> SimilarArtist
		{
			get { return Sound.similarArtists; }
			set
			{
				if (Sound.similarArtists != value)
				{
					Sound.similarArtists = value;
					OnPropertyChanged("SimilarArtist");
				}
			}
		}
		#endregion

		public SoundModelView(Sound sound)
		{
			this.Sound = sound;
		}

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


		#region file fields

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


		public void OnLoadStarted(object sender, Argument state)
		{
			if (!FreezeClick)
				FreezeClick = true;

			CurrentProgressVisibility = true;
		}

		public void OnProgresChanged(object sender, Argument state)
		{

		}

		public void OnLoadEnded(object sender, Argument state)
		{
			CurrentProgressVisibility = false;

			bool result = (bool)state.result;
			if (result)
			{
				State = SyncStates.IsSynced;
			}

			this.Checked = !((bool)state.result);
			this.IsLoadedToDisk = (bool)state.result;

			System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(LoadTagsFromLast), Sound);

		}

		public void OnRaiseError(object sender, Argument state)
		{

		}

		private void LoadTagsFromLast(object SoundObj)
		{
			var sound = SoundObj as Sound;
			if (sound == null) return;

			ArtistWithDetails artist = null;
			try
			{
				artist = Handlers.LastFmHandler.Api.Artist.GetInfo(sound.artist);
				sound.authorPhotoPath = artist.Images[2].Value; // little spike 
			}
			catch (Exception)
			{ }

			if (artist == null)
				return;

			sound.similarArtists = artist.SimilarArtists.Select(el => el.Name).ToList<string>();
		}


		public bool IsEqual(IDownnloadedData data)
		{
			return Sound.IsEqual(data);
		}
	}
}
