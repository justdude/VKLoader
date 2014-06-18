using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Threading;
using System.Collections;
using System.Windows.Media.Imaging;

namespace VKMusicSync.Model
{
    public class Wall
    {
        public int id { get; set; }
        public int from_id { get; set; }
        public int to_id { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public Object comments { get; set; }
        public Object likes { get; set; }
        public Object reposts { get; set; }
    }

    public class Photo
    {
        public string id { get; set; }
        public string album_id { get; set; }
        public string owner_id { get; set; }
        public string photo_75 { get; set; }
        public string photo_130 { get; set; }
        public string photo_1280 { get; set; }
        public string photo_2560 { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string text { get; set; }
        public float date { get; set; }
        public int user_likes { get; set; }

        public string path;

    }

    public class Album
    {
        public int id { get; set; }
        public string title { get; set; }
        public string comment { get; set; }
        public string picture { get; set; }
    }
    public class Friend
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; }
        public string birthdate { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string timezone { get; set; }
        public string photo { get; set; }
        public string photo_medium { get; set; }
        public string photo_big { get; set; }
    }

    public class MyCategory
    {
        public string Name { get; set; }
        public List<ImageModel> Images { get; set; }
    }

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