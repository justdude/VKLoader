using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKMusicSync.Model
{
	public class Sound : SoundBase, IDownnloadedData, IEqualityComparer<Sound>
	{

		#region LastFm

		public string authorPhotoPath { get; set; }
		public List<string> similarArtists { get; set; }
		#endregion

		#region IO

		public double Size { get; set; }
		public double LoadedSize { get; set; }
		public bool IsLoadedToDisk { get; set; }
		#endregion

		public IStateChanged InstanceWithEvents
		{
			get
			{
				return null;
			}
		}

		public Handlers.Synchronize.SyncStates State
		{
			get;
			set;
		}


		public string PathWithFileName
		{
			get
			{
				return Path + @"\" + FileName + FileExtention;
			}
		}

		private string mvFileName;
		public string FileName
		{
			get
			{
				if (string.IsNullOrEmpty(mvFileName) || string.IsNullOrEmpty(mvFileName))
				{
					return this.artist + " - " + this.title;
				}
				return mvFileName;
			}
			set
			{
				mvFileName = value;
			}
		}

		public string FileExtention
		{
			get
			{
				return ".mp3";
			}
		}

		private string mvPath;

		public string Path
		{
			get
			{
				if (string.IsNullOrEmpty(mvPath) || string.IsNullOrEmpty(mvPath))
				{
					return url;
				}
				return mvPath;
			}
			set
			{
				mvPath = value;
			}
		}

		public string MD5
		{
			get;
			set;
		}

		public Sound()
		{
			this.artist = "no name";
			this.title = "track";
		}

		public Sound(string artist, string title)
		{
			this.artist = artist;
			this.title = title;
		}

		public Sound(string url)
		{
			this.artist = "no name";
			this.title = "track";
			this.url = url;
		}

		public Sound(SoundBase p)
		{
			base.artist = p.artist;
			base.duration = p.duration;
			base.genre_id = p.genre_id;
			base.id = p.id;
			base.lyrics_id = p.lyrics_id;
			base.title = p.title;
			base.url = p.url;
		}

		public override string ToString()
		{
			return String.Format("Sound:{0} : {1}", artist, title);
		}

		public override bool Equals(object obj)
		{
			Sound sound = obj as Sound;
			if (sound != null)
			{
				return sound.IsEqual(this);
			}
			return false;
			/* bool isNamesEquals = x.FileName.ToLower() == y.FileName.ToLower();
			 return isNamesEquals;*/
		}

		public int GetHashCode(Sound obj)
		{
			throw new NotImplementedException();
		}

		public bool IsEqual(IDownnloadedData data)
		{
			var sound = data as Sound;
			float coeff = 0;

			bool isNamesEquals = FileName.ToLower().Trim() == data.FileName.ToLower().Trim();
			if (isNamesEquals)
				coeff = 5;

			if (sound != null)
			{
				bool isEqualArtist = artist.ToLower().Trim() == sound.artist.ToLower().Trim();
				bool isEqualSongName = title.ToLower().Trim() == sound.title.ToLower().Trim();

				bool isEqualFileDuration = duration == sound.duration && duration > 0;
				bool isEqualFileSize = Size == sound.Size && Size > 0;

				if (isEqualArtist)
					coeff += 1;
				if (isEqualSongName)
					coeff += 3;

				if (isEqualFileDuration)
					coeff += 0.5f;
				if (isEqualFileSize)
					coeff += 0.5f;

			}

			return coeff >= 4f;
		}

		public bool Equals(Sound x, Sound y)
		{
			throw new NotImplementedException();
		}
	}
}
