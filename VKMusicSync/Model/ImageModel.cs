using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace VKMusicSync.Model
{
	public class ImageModel
	{
		public string ImageId { get; set; }

		public ImageModel(string path)
		{
			_path = path;
			_source = new Uri(path);
			_image = BitmapFrame.Create(_source);
		}

		public override string ToString()
		{
			return _source.ToString();
		}

		private string _path;

		private Uri _source;
		public string Source { get { return _path; } }

		private BitmapFrame _image;
		public BitmapFrame Image { get { return _image; } set { _image = value; } }
	} 
}
