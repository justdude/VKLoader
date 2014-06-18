using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using vkontakte;
using VKMusicSync.Model;
using VKMusicSync.Handlers;

namespace VKMusicSync.Handlers
{

    public enum PhotosSize
    {
        xxx,
        xx,
        x,
        medium,
        small
    }

    public class AudiosContainer
    {
        List<Sound> sounds = new List<Sound>();
        public void Bind(XmlDocument doc)
        {
            sounds.Clear();
            Sound s = new Sound();
            System.Xml.XmlNodeList nodes = doc["response"].ChildNodes;
            //System.Windows.Forms.MessageBox.Show(nodes.Count);
            for (int i = 1; i<nodes.Count; i++)
            {
               // System.Windows.Forms.MessageBox.Show(nodes[i]);
                s = new Sound();
                s.artist = nodes[i]["artist"].InnerText;
                s.title = nodes[i]["title"].InnerText;
                s.duration = (int.Parse(nodes[i]["duration"].InnerText));
                s.url = nodes[i]["url"].InnerText;
                sounds.Add(s);
            }
        }

        public List<Sound> getSound()
        {
            return sounds;
        }
    }

    public class AlbumsContainer
    {
        List<Album> albums = new List<Album>();

        public void Bind(XmlDocument doc)
        {
            albums.Clear();
            Album t = new Album();
            XmlNodeList albumList = doc["response"].ChildNodes;
            foreach (System.Xml.XmlNode album in albumList)
            {
                t = new Album();
                t.id = int.Parse(album["aid"].InnerText);
                t.title = album["title"].InnerText;
                t.picture = album["thumb_id"].InnerText;
                t.comment = album["description"].InnerText;
                albums.Add(t);
            }
        }

        public List<Album> getAlbums()
        {
            return albums;
        }
    }
    public class PhotosAndAlbumBinder:AlbumsContainer 
    {
        List<MyCategory> albums = new List<MyCategory>();

        public void Bind(XmlDocument doc)
        {
            albums.Clear();
            MyCategory t = new MyCategory();
            XmlNodeList albumList = doc["response"].ChildNodes;
            foreach (System.Xml.XmlNode album in albumList)
            {
                t = new MyCategory();
                t.Name = album["title"].InnerText;
                t.Images = this.GetPhotosListFromAlbum(int.Parse(album["aid"].InnerText));
                albums.Add(t);
            }
        }

        public static string GetMaxPhotoAdress(XmlNode node, PhotosSize size)
        {
            if (node["src_xxbig"] != null && size == PhotosSize.xxx) return node["src_xxbig"].InnerText;
            else if (node["src_xbig"] != null && size == PhotosSize.xx) return node["src_xbig"].InnerText;
            else if (node["src_big"] != null && size == PhotosSize.x) return node["src_big"].InnerText;
            else if (node["src"] != null && size == PhotosSize.medium) return node["src_small"].InnerText;
            else if (node["src_small"] != null && size == PhotosSize.small) return node["src"].InnerText;
            string str = GetMaxPhotoAdress(node);
            if (null != str && str != string.Empty) return str;
            return null;
        }

        public static string GetMaxPhotoAdress(XmlNode node)
        {
            if (node["src_xxbig"] != null) return node["src_xxbig"].InnerText;
            else if (node["src_xbig"] != null) return node["src_xbig"].InnerText;
            else if (node["src_big"] != null) return node["src_big"].InnerText;
            else if (node["src"] != null) return node["src_small"].InnerText;
            else if (node["src_small"] != null) return node["src"].InnerText;
            return null;
        }

        public List<ImageModel> GetPhotosListFromAlbum(int albumID) {
            List<ImageModel> photos = new List<ImageModel>();
            XmlNodeList xml = APIManager.vk.GetPhotosFromAlbum(APIManager.vk.UserId, albumID)["response"].ChildNodes;
                foreach (XmlNode node in xml)
                {
                   photos.Add(new ImageModel(GetMaxPhotoAdress(node,PhotosSize.x))); 
                }
            return photos;
        }

        public List<MyCategory> getAlbums()
        {
            return albums;
        }
    }
}
